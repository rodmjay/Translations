#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Translations.Core.Common.Data.Bases;
using Translations.Shared.Enums;

namespace Translations.Core.Entities;

public class MachineTranslation : BaseEntity<MachineTranslation>
{
    public TranslationEngine EngineId { get; set; }
    public Engine Engine { get; set; }

    public string LanguageId { get; set; }
    public EngineLanguage Language { get; set; }

    public int Weight { get; set; }

    public int PhraseId { get; set; }
    public Phrase Phrase { get; set; }
    public DateTime? TranslationDate { get; set; }
    public string Text { get; set; }

    public override void Configure(EntityTypeBuilder<MachineTranslation> builder)
    {
        builder.ToTable(nameof(MachineTranslation), Constants.DefaultSchema);

        builder.HasKey(x => new { x.EngineId, x.LanguageId, x.PhraseId });

        builder.HasOne(x => x.Phrase)
            .WithMany(x => x.MachineTranslations)
            .HasForeignKey(x => x.PhraseId);

        builder.HasOne(x => x.Language)
            .WithMany(x => x.MachineTranslations)
            .HasForeignKey(x => new { x.LanguageId, x.EngineId });

        builder.HasOne(x => x.Engine)
            .WithMany(x => x.MachineTranslations)
            .HasForeignKey(x => x.EngineId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}