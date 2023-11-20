#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Translations.Core.Common.Caching.Extensions;

public static class DistributedCacheExtensions
{
    private static readonly JsonSerializerSettings TurnOffJsonIgnoreSettings = new()
    {
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        ContractResolver = new IgnoreJsonAttributesResolver()
    };

    public static void Create<T>(this IDistributedCache cache, string key, T item)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        cache.Create(key, item,
            new DistributedCacheEntryOptions {AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)});
    }

    public static void Create<T>(this IDistributedCache cache, string key, T item, DateTimeOffset expires)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        cache.Create(key, item,
            new DistributedCacheEntryOptions {AbsoluteExpiration = expires});
    }

    public static void Create<T>(this IDistributedCache cache, string key, T item, TimeSpan slidingExpiration)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        cache.Create(key, item,
            new DistributedCacheEntryOptions {SlidingExpiration = slidingExpiration});
    }

    public static void Create<T>(this IDistributedCache cache, string key, T item,
        DistributedCacheEntryOptions options)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        var json = JsonConvert.SerializeObject(item, TurnOffJsonIgnoreSettings);

        cache.SetString(key, json, options);
    }

    public static T GetOrCreate<T>(this IDistributedCache cache, string key,
        Func<DistributedCacheEntryOptions, T> addItemFactory)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        return cache.GetOrCreate(key, addItemFactory,
            new DistributedCacheEntryOptions {AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)});
    }

    public static T GetOrCreate<T>(this IDistributedCache cache, string key,
        Func<DistributedCacheEntryOptions, T> addItemFactory, DateTimeOffset expires)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        return cache.GetOrCreate(key, addItemFactory,
            new DistributedCacheEntryOptions {AbsoluteExpiration = expires});
    }

    public static T GetOrCreate<T>(this IDistributedCache cache, string key,
        Func<DistributedCacheEntryOptions, T> addItemFactory, TimeSpan slidingExpiration)
    {
        return cache.GetOrCreate(key, addItemFactory,
            new DistributedCacheEntryOptions {SlidingExpiration = slidingExpiration});
    }

    public static T GetOrCreate<T>(this IDistributedCache cache, string key,
        Func<DistributedCacheEntryOptions, T> addItemFactory, DistributedCacheEntryOptions options)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        T item;
        var json = cache.GetString(key);

        if (string.IsNullOrEmpty(json))
        {
            item = addItemFactory(options);

            json = JsonConvert.SerializeObject(item, TurnOffJsonIgnoreSettings);

            cache.SetString(key, json, options);
        }
        else
        {
            item = JsonConvert.DeserializeObject<T>(json, TurnOffJsonIgnoreSettings);
        }

        return item;
    }

    public static Task<T> GetOrCreateAsync<T>(this IDistributedCache cache, string key,
        Func<DistributedCacheEntryOptions, Task<T>> addItemFactory)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        return cache.GetOrCreateAsync(key, addItemFactory,
            new DistributedCacheEntryOptions {AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)});
    }

    public static Task<T> GetOrCreateAsync<T>(this IDistributedCache cache, string key,
        Func<DistributedCacheEntryOptions, Task<T>> addItemFactory, DateTimeOffset expires)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        return cache.GetOrCreateAsync(key, addItemFactory,
            new DistributedCacheEntryOptions {AbsoluteExpiration = expires});
    }

    public static Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key,
        Func<DistributedCacheEntryOptions, Task<T>> addItemFactory, TimeSpan slidingExpiration)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        return cache.GetOrCreateAsync(key, addItemFactory,
            new DistributedCacheEntryOptions {SlidingExpiration = slidingExpiration});
    }

    public static async Task<T> GetOrCreateAsync<T>(this IDistributedCache cache, string key,
        Func<DistributedCacheEntryOptions, Task<T>> addItemFactory, DistributedCacheEntryOptions options)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        T item;
        var json = await cache.GetStringAsync(key);

        if (string.IsNullOrEmpty(json))
        {
            item = await addItemFactory(options);

            json = JsonConvert.SerializeObject(item, TurnOffJsonIgnoreSettings);

            await cache.SetStringAsync(key, json, options);
        }
        else
        {
            item = JsonSerializer.Deserialize<T>(json);
        }

        return item;
    }

    public static async Task RemoveAsync(this IDistributedCache cache, params string[] keys)
    {
        foreach (var key in keys) await cache.RemoveAsync(key, CancellationToken.None);
    }

    public static async Task RemoveAsync(this IDistributedCache cache, params Task<string>[] keys)
    {
        foreach (var key in keys) await cache.RemoveAsync(await key, CancellationToken.None);
    }

    private class IgnoreJsonAttributesResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = base.CreateProperties(type, memberSerialization);
            foreach (var prop in props)
            {
                prop.Ignored = false;
                prop.Writable = true;
            }

            return props;
        }
    }
}