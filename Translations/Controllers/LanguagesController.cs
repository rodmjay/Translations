#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Translations.Common.Bases;
using Translations.Core.Managers;
using Translations.Shared.Interfaces;
using Translations.Shared.Outputs;

namespace Translations.Controllers;

[AllowAnonymous]
public class LanguagesController : BaseController, ILanguagesController
{
    private readonly LanguageManager _languageManager;

    public LanguagesController(IServiceProvider serviceProvider, LanguageManager languageManager) : base(
        serviceProvider)
    {
        _languageManager = languageManager;
    }

    [HttpGet]
    public async Task<List<LanguageOutput>> GetLanguagesAsync()
    {
        return await _languageManager.GetLanguagesAsync<LanguageOutput>().ConfigureAwait(false);
    }

    [HttpGet("all")]
    public async Task<List<LanguagesWithEnginesOutput>> GetAllLanguagesAsync()
    {
        return await _languageManager.GetAllLanguagesAsync<LanguagesWithEnginesOutput>().ConfigureAwait(false);
    }

    [HttpGet("details")]
    public Task<LanguagesWithEnginesOutput> GetLanguageAsync([FromQuery]string languageId)
    {
        return _languageManager.GetLanguageAsync<LanguagesWithEnginesOutput>(languageId);
    }
}