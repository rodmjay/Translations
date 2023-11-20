#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Translations.Core.Common.Settings;

namespace Translations.Core.Common.Middleware.Interfaces;

public interface ICoreAppBuilder
{
    IServiceCollection Services { get; }
    AppSettings AppSettings { get; }
    IConfiguration Configuration { get; }
    string ConnectionString { get; set; }
    ICollection<string> AssembliesToMap { get; set; }
}