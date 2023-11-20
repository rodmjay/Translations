using Translations.Core.Entities;

namespace Translations.Core.Extensions
{
    public static class EngineExtensions
    {
        public static bool HasLanguageEnabled(this Engine engine, string language)
        {
            return engine.Languages.Any(x => x.LanguageId == language);
        }
    }
}
