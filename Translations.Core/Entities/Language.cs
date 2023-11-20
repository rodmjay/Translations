#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Translations.Core.Common.Data.Bases;

namespace Translations.Core.Entities;

[ExcludeFromCodeCoverage]
public class Language : BaseEntity<Language>
{
    public ICollection<EngineLanguage> Engines { get; set; }
    public string Name { get; set; }
    public string Id { get; set; }

    public override void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable(nameof(Language), Constants.DefaultSchema);

        builder.HasKey(x => x.Id);
    }
}