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

public class EnginesController : BaseController, IEnginesController
{
    private readonly EngineManager _engineManager;

    public EnginesController(IServiceProvider serviceProvider, EngineManager engineManager) : base(serviceProvider)
    {
        _engineManager = engineManager;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<List<EngineWithLanguagesOutput>> GetEnginesAsync()
    {
        return await _engineManager.GetEngines<EngineWithLanguagesOutput>();
    }
}