#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Translations.Common.Swagger;

[ExcludeFromCodeCoverage]
public class SwaggerExcludeFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties == null || context.Type == null)
            return;

        var excludedProperties = context.Type.GetProperties()
            .Where(t =>
                t.GetCustomAttribute<JsonIgnoreAttribute>(true)
                != null);

        foreach (var excludedProperty in excludedProperties)
            if (schema.Properties.ContainsKey(excludedProperty.Name))
                schema.Properties.Remove(excludedProperty.Name);
    }
}