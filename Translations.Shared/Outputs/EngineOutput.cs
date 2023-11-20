using Translations.Shared.Enums;

namespace Translations.Shared.Outputs;

public class EngineOutput 
{
    public TranslationEngine Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }

}