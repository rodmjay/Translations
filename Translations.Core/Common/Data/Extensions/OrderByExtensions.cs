#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Shared.Outputs;

namespace Translations.Core.Common.Data.Extensions;

public static class OrderByExtensions
{
    public static IQueryable<TSource> OrderBy<TSource>(
        this IQueryable<TSource> queryable,
        string sorts)
    {
        if (string.IsNullOrWhiteSpace(sorts))
            return queryable;

        var sort = new SortCollection<TSource>(sorts);
        return sort.Apply(queryable);
    }

    public static IQueryable<TSource> OrderBy<TSource>(
        this IQueryable<TSource> queryable,
        params string[] sorts)
    {
        var sort = new SortCollection<TSource>(sorts);
        return sort.Apply(queryable);
    }

    public static IQueryable<TSource> OrderBy<TSource>(
        this IQueryable<TSource> queryable,
        string sorts,
        out SortCollection<TSource> sortCollection)
    {
        sortCollection = new SortCollection<TSource>(sorts);
        return sortCollection.Apply(queryable);
    }
}