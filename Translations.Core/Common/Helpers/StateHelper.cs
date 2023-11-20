#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Translations.Core.Common.Data.Enums;

namespace Translations.Core.Common.Helpers;

[ExcludeFromCodeCoverage]
public class StateHelper
{
    public static EntityState ConvertState(ObjectState state)
    {
        switch (state)
        {
            case ObjectState.Added:
                return EntityState.Added;
            case ObjectState.Modified:
                return EntityState.Modified;
            case ObjectState.Deleted:
                return EntityState.Deleted;
            case ObjectState.Unchanged:
                return EntityState.Unchanged;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    public static ObjectState ConvertState(EntityState state)
    {
        switch (state)
        {
            case EntityState.Detached:
                return ObjectState.Unchanged;

            case EntityState.Unchanged:
                return ObjectState.Unchanged;

            case EntityState.Added:
                return ObjectState.Added;

            case EntityState.Deleted:
                return ObjectState.Deleted;

            case EntityState.Modified:
                return ObjectState.Modified;

            default:
                throw new ArgumentOutOfRangeException(nameof(state));
        }
    }
}