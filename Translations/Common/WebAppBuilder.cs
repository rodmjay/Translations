#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Diagnostics.CodeAnalysis;
using Translations.Core.Common.Middleware.Builders;
using Translations.Core.Common.Settings;

namespace Translations.Common;

[ExcludeFromCodeCoverage]
public class WebAppBuilder
{
    public WebAppBuilder(
        AppBuilder appBuilder,
        IWebHostEnvironment environment)
    {
        Environment = environment;
        AppSettings = appBuilder.AppSettings;
        Services = appBuilder.Services;
        Configuration = appBuilder.Configuration;
        ConnectionString = appBuilder.ConnectionString;
        AssembliesToMap = appBuilder.AssembliesToMap;
    }

    public IWebHostEnvironment Environment { get; }
    public ICollection<string> AssembliesToMap { get; set; }
    public IServiceCollection Services { get; }
    public IConfiguration Configuration { get; }
    public string ConnectionString { get; set; }
    public AppSettings AppSettings { get; set; }
}