#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Runtime.CompilerServices;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Translations.Core.Common.Data.Enums;
using Translations.Core.Common.Data.Interfaces;
using Translations.Core.Common.Services.Bases;
using Translations.Core.Entities;
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

    public Task<List<T>> GetPhrases<T>(int[] phraseIds) where T : PhraseOutput
    {
        return Phrases.Where(x => phraseIds.Contains(x.Id))
            .ProjectTo<T>(ProjectionMapping).ToListAsync();
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
}