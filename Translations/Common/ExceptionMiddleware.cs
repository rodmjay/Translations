#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Translations.Common.Validation;
using Translations.Core.Common.Data.Extensions;

namespace Translations.Common;

public class ExceptionMiddleware
{
    private static string GetLogMessage(string message, [CallerMemberName] string callerName = null)
    {
        return $"[{nameof(ExceptionMiddleware)}.{callerName}] - {message}";
    }

    private readonly JsonSerializerSettings _jsonSerializerSettings;
    private readonly ILoggerFactory _loggerFactory;
    private readonly RequestDelegate _next;


    public ExceptionMiddleware(
        RequestDelegate next,
        ILoggerFactory loggerFactory,
        IOptions<MvcNewtonsoftJsonOptions> jsonOptions)
    {
        _next = next;
        _loggerFactory = loggerFactory;
        _jsonSerializerSettings = jsonOptions.Value.SerializerSettings;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static HttpStatusCode GetErrorCode(Exception e)
    {
        switch (e)
        {
            case UnauthorizedAccessException _:
                return HttpStatusCode.Unauthorized;
            case ValidationException _:
                return HttpStatusCode.BadRequest;
            case FormatException _:
                return HttpStatusCode.BadRequest;
            case AuthenticationException _:
                return HttpStatusCode.Forbidden;
            case NotImplementedException _:
                return HttpStatusCode.NotImplemented;
            default:
                return HttpStatusCode.InternalServerError;
        }
    }

    private void LogAndAddException(ValidationResultModel modelResult, Exception exception)
    {
        var exLogger = _loggerFactory.CreateLogger(exception.TargetSite.DeclaringType.FullName);
        exLogger?.LogError(exception, exception.Message);
        modelResult.Errors.Add(new ValidationError(null, exception.Message));
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var modelResult = new ValidationResultModel();

        if (exception is AggregateException exceptions)
            foreach (var ex in exceptions.InnerExceptions)
                LogAndAddException(modelResult, ex);
        else
            LogAndAddException(modelResult, exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) GetErrorCode(exception);

        return context.Response.WriteAsync(modelResult.ToJson(_jsonSerializerSettings));
    }
}