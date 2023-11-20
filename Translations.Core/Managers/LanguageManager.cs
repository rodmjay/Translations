using Translations.Core.Services;
using Translations.Shared.Outputs;

namespace Translations.Core.Managers;

public class LanguageManager
{
    private readonly ILanguageService _languageService;

    public LanguageManager(
        ILanguageService languageService)
    {
        _languageService = languageService;
    }

    public Task<List<T>> GetLanguagesAsync<T>() where T : LanguageOutput
    {
        return _languageService.GetLanguagesAsync<T>();
    }

    public Task<List<T>> GetAllLanguagesAsync<T>() where T : LanguageOutput
    {
        return _languageService.GetAllLanguagesAsync<T>();
    }

    public Task<T> GetLanguageAsync<T>(string languageId) where T : LanguageOutput
    {
        return _languageService.GetLanguageAsync<T>(languageId);
    }
}