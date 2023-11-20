using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Translations.Core.Common.Services.Bases;
using Translations.Core.Entities;
using Translations.Core.Extensions;
using Translations.Shared.Enums;
using Translations.Shared.Outputs;

namespace Translations.Core.Services;

public class MicrosoftTranslationService : BaseService<Engine>, ITranslationProcessor
{
    private readonly IConfiguration _configuration;
    private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com";
    private static readonly string location = "eastus";
    public MicrosoftTranslationService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider)
    {
        _configuration = configuration;
    }

    private IQueryable<Engine> Engines => Repository.Queryable().Include(x => x.Languages);

    private string GetKey()
    {
        var microsoftTranslateApiKey = Environment.GetEnvironmentVariable("TranslationProMicrosoftApi");
        if (string.IsNullOrEmpty(microsoftTranslateApiKey))
        {
            microsoftTranslateApiKey = _configuration["TranslationProMicrosoftApi"];
        }

        return microsoftTranslateApiKey;
    }

    public async Task Process(
        Dictionary<string, List<string>> languageWithPhrases,
        Func<Dictionary<string, List<GenericTranslationResult>>, TranslationEngine, Task<int>> handleTranslations)
    {
        var retVal = new Dictionary<string, List<GenericTranslationResult>>();

        var engine = await Engines.Where(x => x.Id == TranslationEngine.Azure).FirstAsync();

        foreach (var (languageId, phrases) in languageWithPhrases)
        {
            if (engine.HasLanguageEnabled(languageId))
            {
                var route = $"/translate?api-version=3.0&from=en&to={languageId}";
                var body = phrases.Select(x => new { Text = x }).ToArray();
                var requestBody = JsonConvert.SerializeObject(body);

                using var client = new HttpClient();
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(endpoint + route);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Region", location);
                    request.Headers.Add("Ocp-Apim-Subscription-Key", GetKey());

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var result = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode) continue;

                    var deserializedResult = JsonConvert
                        .DeserializeObject<List<Dictionary<string, List<GenericTranslationResult>>>>(result);

                    if (deserializedResult == null) continue;

                    for (var i = 0; i < deserializedResult.Count; i++)
                    {
                        var originalPhrase = phrases[i];
                        var item = deserializedResult[i];
                        var translationResult = item.First(x => x.Key == "translations");
                            
                        foreach (var translatedPhrase in translationResult.Value)
                        {
                            if (!retVal.ContainsKey(originalPhrase))
                                retVal.Add(originalPhrase, new List<GenericTranslationResult>());

                            retVal[originalPhrase].Add(translatedPhrase);
                        }
                    }
                }
            }
        }

        await handleTranslations(retVal, TranslationEngine.Azure);
    }
}