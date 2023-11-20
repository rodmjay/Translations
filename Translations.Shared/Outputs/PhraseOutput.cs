using Translations.Shared.Options;

namespace Translations.Shared.Outputs;

public class PhraseOutput
{
    public int Id { get; set; }
    public string Text { get; set; }
    public List<MachineTranslationOutput> MachineTranslations { get; set; }
}