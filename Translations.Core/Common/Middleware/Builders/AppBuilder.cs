#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Translations.Core.Common.Settings;

namespace Translations.Core.Common.Middleware.Builders;

public class AppBuilder
{
    public AppBuilder(
        IServiceCollection services,
        AppSettings settings,
        IConfiguration configuration)
    {
        Services = services;
        Configuration = configuration;
        AppSettings = settings;
        AssembliesToMap = new List<string>();
        AzureServiceBusConnectionString = configuration.GetConnectionString("AzureServiceBusConnection");
    }
    public string AzureServiceBusConnectionString { get; set; }

    public List<string> AssembliesToMap { get; set; }
    public IServiceCollection Services { get; }
    public IConfiguration Configuration { get; }
    public string ConnectionString { get; set; }
    public AppSettings AppSettings { get; set; }
}