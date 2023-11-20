#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Translations.Core.Common.Data.Interfaces;
using Translations.Core.Common.Queries;
using Translations.Core.Common.Services.Interfaces;
using Translations.Shared.Outputs;

namespace Translations.Core.Common.Data.Extensions;

public static class DataPagerExtension
{
    public static async Task<PagedList<TOutput>> PaginateAsync<TEntity, TOutput>(
        this IService<TEntity> service,
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, bool>> query,
        PagingQuery paging)
        where TEntity : class, IObjectState
    {
        var combinedExpr = PredicateBuilder.True<TEntity>();

        combinedExpr = combinedExpr.And(filter);
        combinedExpr = combinedExpr.And(query);

        return await service.PaginateAsync<TEntity, TOutput>(combinedExpr, paging);
    }

    public static async Task<PagedList<TOutput>> PaginateAsync<TEntity, TOutput>(
        this IService<TEntity> service,
        Expression<Func<TEntity, bool>> filter,
        PagingQuery paging)
        where TEntity : class, IObjectState
    {
        var paged = new PagedList<TOutput>();

        paging.Page = paging.Page <= 0 ? 1 : paging.Page;
        paging.Size = paging.Size > 0 ? paging.Size : 10;

        paged.CurrentPage = paging.Page;
        paged.PageSize = paging.Size;

        var totalCount = await service.Repository.Queryable()
            .Where(filter)
            .CountAsync();

        var startRow = (paging.Page - 1) * paging.Size;

        paged.Items = await service.Repository.Queryable()
            .Where(filter)
            .OrderBy(paging.Sort)
            .ProjectTo<TOutput>(service.ProjectionMapping)
            .Skip(startRow)
            .Take(paging.Size)
            .ToListAsync();


        paged.TotalItems = totalCount;
        paged.TotalPages = (int) Math.Ceiling(paged.TotalItems / (double) paging.Size);

        return paged;
    }
}