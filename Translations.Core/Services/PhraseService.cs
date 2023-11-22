#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Translations.Core.Common.Data.Enums;
using Translations.Core.Common.Data.Interfaces;
using Translations.Core.Common.Services.Bases;
using Translations.Core.Entities;
using Translations.Core.Models;
using Translations.Shared.Outputs;

namespace Translations.Core.Services;

public partial class PhraseService : BaseService<Phrase>, IPhraseService
{
    private static string GetLogMessage(string message, [CallerMemberName] string callerName = null)
    {
        return $"[{nameof(PhraseService)}.{callerName}] - {message}";
    }

    private readonly ILogger<PhraseService> _logger;
    private readonly IRepositoryAsync<Language> _languageRepository;

    public PhraseService(IServiceProvider serviceProvider, ILogger<PhraseService> logger) : base(serviceProvider)
    {
        _logger = logger;
        _languageRepository = UnitOfWork.RepositoryAsync<Language>();
    }

    private IQueryable<Phrase> Phrases => Repository.Queryable()
        .Include(x => x.MachineTranslations);
    
    private IQueryable<Language> Languages => _languageRepository
        .Queryable().Include(x => x.Engines).ThenInclude(x => x.Engine);

    public async Task<int> EnsurePhrasesWithLanguage(int[] phraseIds, string languageId)
    {
        return await EnsurePhrasesWithLanguages(phraseIds, new[] { languageId });
    }

    public async Task<int> EnsurePhrasesWithLanguages(int[] phraseIds, string[] languageIds)
    {
        var phrases = await Phrases.Where(x => phraseIds.Contains(x.Id)).ToListAsync();
        var languages = await Languages.Where(x => languageIds.Contains(x.Id)).ToListAsync();

        foreach (var phrase in phrases)
        {
            foreach (var language in languages)
            {
                foreach (var engine in language.Engines.Where(x=>x.Engine.Enabled))
                {
                    var machineTranslation = phrase.MachineTranslations
                        .FirstOrDefault(x => x.EngineId == engine.EngineId
                                             && x.LanguageId == language.Id);

                    if (machineTranslation != null) continue;

                    phrase.MachineTranslations.Add(new MachineTranslation()
                    {
                        EngineId = engine.EngineId,
                        LanguageId = language.Id,
                        ObjectState = ObjectState.Added
                    });

                    phrase.ObjectState = ObjectState.Modified;
                }
            }

            if (phrase.ObjectState == ObjectState.Modified)
            {
                Repository.InsertOrUpdateGraph(phrase);
            }
        }

        var records = Repository.Commit();
        return records;
    }

    public async Task<List<T>> GetPhrases<T>(int[] phraseIds, string[] languageIds) where T : PhraseOutput
    {
        var phrases = await Phrases.Where(x => phraseIds.Contains(x.Id))
            .ProjectTo<T>(ProjectionMapping).ToListAsync();

        foreach (var phrase in phrases)
        {
            phrase.MachineTranslations =
                phrase.MachineTranslations.Where(x => languageIds.Contains(x.LanguageId)).ToList();
        }

        return phrases;
    }
    
    public async Task<Dictionary<int, string>> GetPhraseTexts(int[] phraseIds)
    {
        var phrases = await Phrases.Where(x => phraseIds.Contains(x.Id)).ToListAsync();

        return phrases.ToDictionary(x => x.Id, x => x.Text);
    }

    public async Task<int> EnsurePhraseWithLanguages(int phraseId, string[] languageIds)
    {
        return await EnsurePhrasesWithLanguages(new[] { phraseId }, languageIds);
    }


    public async Task<Result> EnsurePhrase(string text)
    {
        string normalizedText = text.ToUpper();

        var phrase = await Phrases.Where(x => x.NormalizedText == normalizedText).FirstOrDefaultAsync();
        if (phrase != null) return Result.Success(phrase.Id);

        phrase = new Phrase()
        {
            Text = text,
            NormalizedText = normalizedText
        };

        var records = Repository.Insert(phrase, true);
        if (records > 0)
        {
            return Result.Success(phrase.Id);
        }

        return Result.Failed();
    }


    public async Task<EnsurePhrasesResult> EnsurePhrases(string[] input)
    {
        var texts = input
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct()
            .ToArray();

        var normalizedTexts = texts.Select(x => x.ToUpper()).ToArray();

        var retVal = new EnsurePhrasesResult()
        {
            PhrasesRequested = texts.Length,
        };

        var existingPhrases = await Phrases.Where(x => normalizedTexts.Contains(x.NormalizedText)).ToListAsync();

        retVal.ExistingPhrases = existingPhrases.Count;

        foreach (var text in texts)
        {
            if (existingPhrases.Any(x => x.NormalizedText == text.ToUpper())) continue;

            var phrase = new Phrase()
            {
                Text = text,
                NormalizedText = text.ToUpper()
            };

            Repository.Insert(phrase);
        }

        retVal.PhrasesAdded = Repository.Commit();

        retVal.Phrases = await Phrases.Where(x => texts.Contains(x.Text)).Select(x => x.Id).ToArrayAsync();

        return retVal;
    }
}