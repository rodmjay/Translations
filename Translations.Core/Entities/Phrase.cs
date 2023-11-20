#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Translations.Core.Common.Data.Bases;
using Translations.Core.Common.Data.Interfaces;

namespace Translations.Core.Entities;

[ExcludeFromCodeCoverage]
public class Phrase : BaseEntity<Phrase>, ICreated
{
    public Phrase()
    {
        MachineTranslations = new List<MachineTranslation>();
    }

    public int Id { get; set; }
    public string Text { get; set; }
    public DateTimeOffset Created { get; set; }

    public ICollection<MachineTranslation> MachineTranslations { get; set; }

    public override void Configure(EntityTypeBuilder<Phrase> builder)
    {
        builder.ToTable(nameof(Phrase), Constants.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1);
    }
    
}