#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Azure.Messaging.ServiceBus;
using Google.Cloud.Translation.V2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Translations.Core.Common.Middleware.Builders;
using Translations.Core.Errors;
using Translations.Core.Managers;
using Translations.Core.Services;

namespace Translations.Core.Extensions;

public static class AppBuilderExtensions
{

    public static AppBuilder AddTranslationDependencies(this AppBuilder builder)
    {
        builder.Services.TryAddTransient<PhraseErrorDescriber>();
        builder.Services.TryAddTransient<TranslationErrorDescriber>();

        builder.Services.TryAddScoped<IPhraseService, PhraseService>();
        builder.Services.TryAddScoped<IMachineTranslationService, MachineTranslationService>();
        builder.Services.TryAddScoped<IEngineService, EngineService>();
        builder.Services.TryAddScoped<ILanguageService, LanguageService>();

        builder.Services.TryAddScoped<LanguageManager>();
        builder.Services.TryAddScoped<TranslationManager>();
        builder.Services.TryAddScoped<EngineManager>();

        builder.Services.TryAddSingleton(x =>
        {
            var googleTranslateApiKey = Environment.GetEnvironmentVariable("TranslationProGoogleApi");
            if (string.IsNullOrEmpty(googleTranslateApiKey))
            {
                googleTranslateApiKey = x.GetRequiredService<IConfiguration>()["TranslationProGoogleApi"];
            }

            var apiKey = googleTranslateApiKey;
            var client = TranslationClient.CreateFromApiKey(apiKey);
            return client;
        });

        builder.Services.TryAddScoped<MicrosoftTranslationService>();
        builder.Services.TryAddScoped<GoogleTranslationService>();

        builder.Services.TryAddSingleton(x =>
        {
            var configuration = x.GetRequiredService<IConfiguration>();
            return new ServiceBusClient(configuration.GetConnectionString("AzureServiceBusConnection"));
        });
        return builder;
    }
}