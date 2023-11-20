#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Core.Common.Services.Interfaces;
using Translations.Core.Entities;
using Translations.Shared.Outputs;

namespace Translations.Core.Services;

public interface ILanguageService : IService<Language>
{
    Task<List<T>> GetLanguagesAsync<T>() where T : LanguageOutput;
    Task<List<T>> GetAllLanguagesAsync<T>() where T : LanguageOutput;
    Task<T> GetLanguageAsync<T>(string languageId) where T : LanguageOutput;
}