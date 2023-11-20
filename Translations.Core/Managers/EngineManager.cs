#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Core.Services;
using Translations.Shared.Outputs;

namespace Translations.Core.Managers;

public class EngineManager
{
    private readonly IEngineService _engineService;

    public EngineManager(IEngineService engineService)
    {
        _engineService = engineService;
    }

    public Task<List<T>> GetEngines<T>() where T : EngineOutput
    {
        return _engineService.GetEngines<T>();
    }
}