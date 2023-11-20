using Google.Cloud.Translation.V2;
using Microsoft.EntityFrameworkCore;
using Translations.Core.Common.Services.Bases;
using Translations.Core.Entities;
using Translations.Core.Extensions;
using Translations.Shared.Enums;
using Translations.Shared.Outputs;

namespace Translations.Core.Services
{
    public class GoogleTranslationService : BaseService<Engine>, ITranslationProcessor
    {
        private readonly TranslationClient _googleClient;

        public GoogleTranslationService(IServiceProvider serviceProvider, TranslationClient googleClient) : base(serviceProvider)
        {
            _googleClient = googleClient;
        }

        private IQueryable<Engine> Engines => Repository.Queryable().Include(x => x.Languages);

        public async Task Process(
            Dictionary<string, List<string>> languageWithPhrases,
            Func<Dictionary<string, List<GenericTranslationResult>>, TranslationEngine, Task<int>>
                handleTranslations)
        {
            var engine = await Engines.Where(x => x.Id == TranslationEngine.Google).FirstAsync();

            foreach (var (languageId, phrases) in languageWithPhrases)
            {
                if (engine.HasLanguageEnabled(languageId))
                {
                    var texts = phrases.Select(x => x.ToString()).ToArray();
                    var translations = await _googleClient.TranslateTextAsync(texts, languageId);
                    var dictionary = translations.GroupBy(x => x.OriginalText).ToDictionary(x => x.Key, x => x.ToList()
                        .Select(a =>
                            new GenericTranslationResult()
                            {
                                Text = a.TranslatedText,
                                To = languageId
                            }).ToList());


                    await handleTranslations(dictionary, TranslationEngine.Google);
                }
            }

        }
    }
}
