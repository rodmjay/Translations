using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Translations.Core.Common.Data.Bases;
using Translations.Shared.Enums;

namespace Translations.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class Engine : BaseEntity<Engine>
    {
        public TranslationEngine Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }

        public ICollection<EngineLanguage> Languages { get; set; }
        public ICollection<MachineTranslation> MachineTranslations { get; set; }

        public override void Configure(EntityTypeBuilder<Engine> builder)
        {
            builder.ToTable(nameof(Engine), Constants.DefaultSchema);

            builder.HasKey(x => x.Id);

        }
    }
}
