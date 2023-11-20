namespace Translations.Shared.Outputs;

public class LanguagesWithEnginesOutput : LanguageOutput
{
    public ICollection<EngineOutput> Engines { get; set; }
}