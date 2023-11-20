using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Translations.Core.Services;
using Translations.Shared.Enums;
using Translations.Shared.Options;
using Translations.Shared.Outputs;

namespace Translations.Core.Managers;


public class TranslationManager
{
    private static string GetLogMessage(string message, [CallerMemberName] string callerName = null)
    {
        return $"[{nameof(TranslationManager)}.{callerName}] - {message}";
    }

    private readonly IPhraseService _phraseService;
    private readonly GoogleTranslationService _googleService;
    private readonly MicrosoftTranslationService _microsoftService;
    private readonly IMachineTranslationService _machineTranslationService;
    private readonly ILogger<TranslationManager> _logger;

    public TranslationManager(
        IPhraseService phraseService,
        GoogleTranslationService googleService,
        MicrosoftTranslationService microsoftService,
        IMachineTranslationService machineTranslationService,
        ILogger<TranslationManager> logger)
    {
        _phraseService = phraseService;
        _googleService = googleService;
        _microsoftService = microsoftService;
        _machineTranslationService = machineTranslationService;
        _logger = logger;
    }

    public async Task<List<T>> Translate<T>(PhraseBulkCreateOptions input) where T: PhraseOutput
    {
        var createPhrases = await _phraseService.EnsurePhrases(input.Texts);
        
        await _phraseService.EnsurePhrasesWithLanguages(createPhrases.Phrases, input.LanguageIds);

        var pending = await _machineTranslationService.GetPendingTranslations(createPhrases.Phrases);

        foreach (var (engine, phraseWithLanguages) in pending)
        {
            switch (engine)
            {
                case TranslationEngine.Amazon:
                    break;

                case TranslationEngine.Google:
                    await _googleService.Process(phraseWithLanguages, _machineTranslationService.SaveTranslationsAsync);
                    break;

                case TranslationEngine.Azure:
                    await _microsoftService.Process(phraseWithLanguages, _machineTranslationService.SaveTranslationsAsync);
                    break;
            }
        }

        return await _phraseService.GetPhrases<T>(createPhrases.Phrases, input.LanguageIds);
    }
    

}