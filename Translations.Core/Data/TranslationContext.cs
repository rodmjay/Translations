using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Translations.Core.Common.Data.Bases;
using Translations.Core.Entities;
using Translations.Core.Seeding.Extensions;

namespace Translations.Core.Data
{
    public class TranslationContext : BaseContext<TranslationContext>
    {
        private readonly ILoggerFactory _loggerFactory;

        public TranslationContext(
            DbContextOptions<TranslationContext> options, ILoggerFactory loggerFactory) :
            base(options)
        {
            _loggerFactory = loggerFactory;
        }


        public TranslationContext(
            DbContextOptions<TranslationContext> options) : this(options, null)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_loggerFactory != null) optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void SeedDatabase(ModelBuilder builder)
        {
            builder.Entity<Language>().Seed("languages.csv");
            builder.Entity<Engine>().Seed("engines.csv");
            builder.Entity<EngineLanguage>().Seed("engineLanguages.csv");
        }

        protected override void ConfigureDatabase(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
        
    }
}
