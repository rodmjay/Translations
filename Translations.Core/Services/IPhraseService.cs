#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Core.Common.Services.Interfaces;
using Translations.Core.Entities;
using Translations.Core.Models;
using Translations.Shared.Outputs;

namespace Translations.Core.Services;

public interface IPhraseService : IService<Phrase>
{
    Task<Dictionary<int, string>> GetPhraseTexts(int[] phraseIds);
    Task<Result> EnsurePhrase(string text);
    Task<int> EnsurePhraseWithLanguages(int phraseId, string[] languageIds);
    Task<int> EnsurePhrasesWithLanguage(int[] phraseIds, string languageId);

    Task<int> EnsurePhrasesWithLanguages(int[] phraseIds, string[] languageIds);

    Task<EnsurePhrasesResult> EnsurePhrases(string[] input);

    Task<List<T>> GetPhrases<T>(int[] phraseIds, string[] languageIds) where T: PhraseOutput;
}