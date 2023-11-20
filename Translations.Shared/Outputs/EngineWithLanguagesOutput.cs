#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

namespace Translations.Shared.Outputs;

public class EngineWithLanguagesOutput : EngineOutput
{
    public ICollection<LanguageOutput> Languages { get; set; }
}