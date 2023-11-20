#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

namespace Translations.Shared.Options;

public class MachineTranslationOutput 
{
    public string Text { get; set; }
    public string Engine { get; set; }
    public string LanguageId { get; set; }

    public DateTime? TranslationDate { get; set; }
}