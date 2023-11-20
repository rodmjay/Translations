#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Diagnostics.CodeAnalysis;

namespace Translations.Core.Common.Caching;

[ExcludeFromCodeCoverage]
public class CacheSettings
{
    public TimeSpan? DefaultExpiration { get; set; }
}