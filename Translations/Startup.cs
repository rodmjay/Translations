#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Reflection;
using Microsoft.Extensions.Options;
using Translations.Common;
using Translations.Core.Common.Middleware.Extensions;
using Translations.Core.Common.Settings;
using Translations.Core.Data;
using Translations.Core.Extensions;

namespace Translations;

public class Startup
{
    private readonly HttpMessageHandler _identityServerMessageHandler;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment,
        HttpMessageHandler identityServerMessageHandler = null)
    {
        _identityServerMessageHandler = identityServerMessageHandler;
        Configuration = configuration;
        Environment = environment;
    }

    public IWebHostEnvironment Environment { get; }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var builder = services.ConfigureApp(Configuration).AddDatabase<TranslationContext>()
            .AddAutomapperProfilesFromAssemblies()
            .AddCaching()
            .AddTranslationDependencies();


        var webAppBuilder = builder.ConfigureWebApp(Environment);

        var restBuilder = webAppBuilder.ConfigureRest()
            //.AddAuthorization(policy =>
            //{
            //    policy.RequireAuthenticatedUser();
            //    policy.RequireClaim("scope", builder.AppSettings.Audience);
            //    policy.RequireClaim("scope", "openid");
            //    policy.RequireClaim("scope", "profile");
            //})
            //.AddBearerAuthentication(options =>
            //{
            //    options.RequireHttpsMetadata = false;
            //    options.Authority = builder.AppSettings.Authority;
            //    options.Audience = builder.AppSettings.Audience;

            //    if (_identityServerMessageHandler != null)
            //        options.BackchannelHttpHandler = _identityServerMessageHandler;

            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateAudience = false,
            //        ValidAudience = builder.AppSettings.Audience,

            //        NameClaimType = "name",
            //        RoleClaimType = "role"
            //    };

            //    options.Events = new JwtBearerEvents
            //    {
            //        OnAuthenticationFailed = c =>
            //        {
            //            var logger = c.HttpContext.RequestServices.GetRequiredService<ILogger<StartupBase>>();
            //            logger.LogTrace("Authentication Failure");
            //            return Task.FromResult(0);
            //        },
            //        OnTokenValidated = c =>
            //        {
            //            var logger = c.HttpContext.RequestServices.GetRequiredService<ILogger<StartupBase>>();
            //            logger.LogTrace("Authentication Success");
            //            return Task.FromResult(0);
            //        }
            //    };
            //})
            .AddSwagger(Assembly.GetAssembly(GetType()))
            .AddCors(
                builder.AppSettings.Authority,
                builder.AppSettings.AppUrl);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TranslationContext context,
        IOptions<AppSettings> settings)
    {
        RestApiBuilderExtensions.Configure(app, env, context, settings);
    }
}