#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Translations.Common.Validation.Interfaces;

namespace Translations.Common.Validation;

[ExcludeFromCodeCoverage]
public static class ValidationEngine
{
    /// <summary>
    ///     Will validate an entity that implements IValidatableObject and DataAnnotations
    /// </summary>
    /// <typeparam name="T">The type that inherits the abstract basetype DomainObject</typeparam>
    /// <param name="entity">The Entity to validate</param>
    /// <returns></returns>
    public static IValidationContainer<T> GetValidationContainer<T>(this T entity) where T : IValidatableObject
    {
        var brokenRules = new Dictionary<string, IList<string>>();

        var customErrors = entity.Validate(new ValidationContext(entity, null, null));
        foreach (var customError in customErrors)
        foreach (var memberName in customError.MemberNames)
        {
            if (!brokenRules.ContainsKey(memberName)) brokenRules.Add(memberName, new List<string>());

            brokenRules[memberName].Add(customError.ErrorMessage);
        }

        // DataAnnotations
        foreach (var pi in entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        foreach (
            var attribute in (ValidationAttribute[]) pi.GetCustomAttributes(typeof(ValidationAttribute), false)
        )
        {
            if (attribute.IsValid(pi.GetValue(entity, null))) continue;

            if (!brokenRules.ContainsKey(pi.Name)) brokenRules.Add(pi.Name, new List<string>());

            brokenRules[pi.Name].Add(attribute.FormatErrorMessage(pi.Name));
        }

        return new ValidationContainer<T>(brokenRules, entity);
    }
}