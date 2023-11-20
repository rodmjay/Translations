#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Translations.Core.Common.Data.Interfaces;
using Translations.Core.Data;

namespace Translations.Core.Common.Data.Factories;

public class OperationalContextFactory : IApplicationContextFactory
{
    public TranslationContext CreateDbContext(string[] args)
    {
        // Used only for EF .NET Core CLI tools (update database/migrations etc.)
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("sharedSettings.json", false, true);

        var config = builder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<TranslationContext>()
            .EnableSensitiveDataLogging()
            .UseSqlServer(config.GetConnectionString("DefaultConnection"));

        return new TranslationContext(optionsBuilder.Options);
    }
}