#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Globalization;
using System.Reflection;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Translations.Core.Data;

namespace Translations.Core.Seeding.Extensions;

public static class SeedingExtensions
{
    private static readonly string _seederPath;
    private static readonly Assembly _assembly;

    static SeedingExtensions()
    {
        _assembly = typeof(TranslationContext).GetTypeInfo().Assembly;
        _seederPath = $"{_assembly.GetName().Name}.Seeding.csv";
    }

    private static string GetResourceFilename(string resouce)
    {
        return $"{_seederPath}.{resouce}";
    }

    private static CsvReader GetReader(StreamReader reader)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HeaderValidated = null,
            MissingFieldFound = null,
            UseNewObjectForNullReferenceMembers = false,
            IgnoreReferences = true
        };

        var csvReader = new CsvReader(reader, config);

        csvReader.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add("NULL");
        csvReader.Context.TypeConverterOptionsCache.GetOptions<int?>().NullValues.Add("NULL");
        csvReader.Context.TypeConverterOptionsCache.GetOptions<DateTime?>().NullValues.Add("NULL");
        csvReader.Context.TypeConverterOptionsCache.GetOptions<decimal?>().NullValues.Add("NULL");
        csvReader.Context.TypeConverterOptionsCache.GetOptions<bool>().BooleanFalseValues.Add("0");
        csvReader.Context.TypeConverterOptionsCache.GetOptions<bool>().BooleanTrueValues.Add("1");

        return csvReader;
    }

    public static void Seed<TEntity>(this EntityTypeBuilder<TEntity> builder, string fileName)
        where TEntity : class
    {
        var file = GetResourceFilename(fileName);
        using (var stream = _assembly.GetManifestResourceStream(file))
        {
            if (stream == null) return;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var csvReader = GetReader(reader);

                var records = csvReader.GetRecords<TEntity>().ToList();
                builder.HasData(records);
            }
        }
    }
}