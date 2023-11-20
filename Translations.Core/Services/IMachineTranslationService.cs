#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Core.Common.Services.Interfaces;
using Translations.Core.Entities;
using Translations.Shared.Enums;
using Translations.Shared.Outputs;

namespace Translations.Core.Services;

public interface IMachineTranslationService : IService<MachineTranslation>
{
    Task<Dictionary<TranslationEngine, Dictionary<string, List<string>>>> GetPendingTranslations(int[] phraseIds);
    Task<Result> AdjustWeights(int phraseId, string languageId, string oldText, string newText);
    Task<int> SaveTranslationsAsync(Dictionary<string, List<GenericTranslationResult>> input, TranslationEngine engine);
}