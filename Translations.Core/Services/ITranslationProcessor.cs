#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Shared.Enums;
using Translations.Shared.Outputs;

namespace Translations.Core.Services;

public interface ITranslationProcessor
{
    Task Process(Dictionary<string, List<string>> languageWithPhrases,
        Func<Dictionary<string, List<GenericTranslationResult>>, TranslationEngine, Task<int>>
            handleTranslations);
}