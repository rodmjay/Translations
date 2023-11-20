#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion


namespace Translations.Core.Models;

public class EnsurePhrasesResult
{
    public int PhrasesRequested { get; set; }
    public int PhrasesAdded { get; set; }
    public int ExistingPhrases { get; set; }

    public int[] Phrases { get; set; }
}