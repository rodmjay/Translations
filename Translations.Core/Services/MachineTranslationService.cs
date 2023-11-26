#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Translations.Core.Common.Data.Enums;
using Translations.Core.Common.Data.Interfaces;
using Translations.Core.Common.Services.Bases;
using Translations.Core.Entities;
using Translations.Shared.Enums;
using Translations.Shared.Outputs;

namespace Translations.Core.Services;

public class MachineTranslationService : BaseService<MachineTranslation>, IMachineTranslationService
{
    private static string GetLogMessage(string message, [CallerMemberName] string callerName = null)
    {
        return $"[{nameof(MachineTranslationService)}.{callerName}] - {message}";
    }

    private readonly ILogger<MachineTranslationService> _logger;
    private readonly IRepositoryAsync<Phrase> _phraseRepository;

    public MachineTranslationService(IServiceProvider serviceProvider, 
        ILogger<MachineTranslationService> logger) : base(serviceProvider)
    {
        _logger = logger;
        _phraseRepository = UnitOfWork.RepositoryAsync<Phrase>();
    }

    private IQueryable<Phrase> Phrases =>
        _phraseRepository.Queryable()
            .Include(x => x.MachineTranslations)
            .ThenInclude(x => x.Engine);

    private IQueryable<MachineTranslation> MachineTranslations => Repository.Queryable()
        .Include(x => x.Phrase).Include(x => x.Engine);

    public async Task<int> SaveTranslationsAsync(Dictionary<string, List<GenericTranslationResult>> input, TranslationEngine engine)
    {
        _logger.LogInformation(GetLogMessage("Saving {0} Translations: {1}"), engine, input);

        foreach (var (originalText, translations) in input)
        {
            var phrase = await Phrases.Where(x => x.Text == originalText).FirstOrDefaultAsync();
            if (phrase != null)
            {
                foreach (var translation in translations)
                {
                    _logger.LogInformation(GetLogMessage("Saving Translation: {0} to engine: {1} with language: {2}"), phrase.Text, engine, translation.To);

                    var language = translation.To;
                    var machineTranslation = phrase.MachineTranslations
                        .First(x => x.EngineId == engine && x.LanguageId == language);
                    machineTranslation.Text = translation.Text;
                    machineTranslation.TranslationDate = DateTime.UtcNow;
                    machineTranslation.ObjectState = ObjectState.Modified;
                }
                _phraseRepository.InsertOrUpdateGraph(phrase);
            }
        }

        return _phraseRepository.Commit();
    }


    public async Task<Dictionary<TranslationEngine, Dictionary<string, List<string>>>> GetPendingTranslations(int[] phraseIds, string[] languageIds)
    {
        var translations = await MachineTranslations
            .Where(x => x.Engine.Enabled && phraseIds.Contains(x.PhraseId) && languageIds.Contains(x.LanguageId) && x.Text == null)
            .ToListAsync();

        var retVal = translations.GroupBy(x => x.EngineId).ToDictionary(x => x.Key,
            x => x.GroupBy(a => a.LanguageId)
                .ToDictionary(b => b.Key, c => c.Select(d => d.Phrase.Text).ToList()));

        return retVal;
    }

    public async Task<Result> AdjustWeights(int phraseId, string languageId, string oldText, string newText)
    {
        var phrase = await Phrases.Where(x => x.Id == phraseId).FirstAsync();

        var oldTranslation = phrase.MachineTranslations.FirstOrDefault(x => x.Text == oldText && x.LanguageId == languageId);

        var newTranslation = phrase.MachineTranslations.FirstOrDefault(x => x.Text == newText && x.LanguageId == languageId);

        if ((oldTranslation == null || newTranslation == null) || oldTranslation.EngineId == newTranslation.EngineId) 
            return Result.Success();
        
        phrase.ObjectState = ObjectState.Modified;

        oldTranslation.Weight -= 1;
        oldTranslation.ObjectState = ObjectState.Modified;

        newTranslation.Weight += 1;
        newTranslation.ObjectState = ObjectState.Modified;

        var records = _phraseRepository.InsertOrUpdateGraph(phrase, true);

        return records > 0 ? Result.Success(phraseId) : Result.Failed();
    }

}