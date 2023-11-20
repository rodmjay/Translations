#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Shared.Outputs;

namespace Translations.Shared.Interfaces;

public interface IEnginesController
{
    Task<List<EngineWithLanguagesOutput>> GetEnginesAsync();
}