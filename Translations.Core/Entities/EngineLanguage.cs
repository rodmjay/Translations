#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Translations.Core.Common.Data.Bases;
using Translations.Shared.Enums;

namespace Translations.Core.Entities;

[ExcludeFromCodeCoverage]
public class EngineLanguage : BaseEntity<EngineLanguage>
{
    public string LanguageId { get; set; }
    public Language Language { get; set; }

    public TranslationEngine EngineId { get; set; }
    public Engine Engine { get; set; }

    public int Weight { get; set; }

    public ICollection<MachineTranslation> MachineTranslations { get; set; }

    public override void Configure(EntityTypeBuilder<EngineLanguage> builder)
    {
        builder.ToTable(nameof(EngineLanguage), Constants.DefaultSchema);

        builder.HasKey(x => new { x.LanguageId, x.EngineId });

        builder.HasOne(x => x.Language)
            .WithMany(x => x.Engines)
            .HasForeignKey(x => x.LanguageId);

        builder.HasOne(x => x.Engine)
            .WithMany(x => x.Languages)
            .HasForeignKey(x => x.EngineId);
    }
}