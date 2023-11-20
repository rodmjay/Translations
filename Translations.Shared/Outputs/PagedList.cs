#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

namespace Translations.Shared.Outputs;

public class PagedList<TModel>
{
    private const int MaxPageSize = 500;
    private int _pageSize;

    public PagedList()
    {
        Items = new List<TModel>();
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public int CurrentPage { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public IList<TModel> Items { get; set; }
}