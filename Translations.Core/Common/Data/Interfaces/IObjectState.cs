#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Core.Common.Data.Enums;

namespace Translations.Core.Common.Data.Interfaces;

public interface IObjectState
{
    public ObjectState ObjectState { get; set; }
}