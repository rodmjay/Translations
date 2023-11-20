#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Shared.Outputs;

namespace Translations.Shared.Interfaces;

public interface ILanguagesController
{
    Task<List<LanguageOutput>> GetLanguagesAsync();
    Task<List<LanguagesWithEnginesOutput>> GetAllLanguagesAsync();

    Task<LanguagesWithEnginesOutput> GetLanguageAsync(string languageId);
}