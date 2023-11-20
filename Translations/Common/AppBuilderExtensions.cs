using Serilog;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Translations.Core.Common.Middleware.Builders;

namespace Translations.Common
{
    public static class AppBuilderExtensions
    {
        private static string GetLogMessage(string message, [CallerMemberName] string callerName = null)
        {
            return $"[{nameof(AppBuilderExtensions)}.{callerName}] - {message}";
        }
        public static WebAppBuilder ConfigureWebApp(this AppBuilder builder, IWebHostEnvironment environment)
        {
            Log.Logger.Debug(GetLogMessage($"Environment: {environment.EnvironmentName}"));

            return new WebAppBuilder(builder, environment);
        }

        public static RestApiBuilder AddAuthorization(this RestApiBuilder builder,
            Action<AuthorizationPolicyBuilder> action)
        {
            builder.Services.AddAuthorization(options => { options.AddPolicy("ApiScope", action); });

            return builder;
        }

        public static RestApiBuilder AddBearerAuthentication(this RestApiBuilder builder,
            Action<JwtBearerOptions> action)
        {
            //builder.Services.AddSingleton<IClaimsTransformation, ClaimsTransformer>();
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", action);

            return builder;
        }

    }
}
