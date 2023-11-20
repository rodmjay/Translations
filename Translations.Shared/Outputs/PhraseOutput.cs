using Translations.Shared.Options;

namespace Translations.Shared.Outputs;

public class PhraseOutput
{
    public string Text { get; set; }
    public List<MachineTranslationOutput> MachineTranslations { get; set; }
}