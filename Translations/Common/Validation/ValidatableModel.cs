#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Translations.Common.Validation;

[ExcludeFromCodeCoverage]
public abstract class ValidatableModel : IValidatableObject
{
    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Override this method to implement custom validation in your entities
        // This is only for making it compile... and returning null will give an exception.
        if (false)
#pragma warning disable 162
            yield return new ValidationResult("Well, this should not happened...");
#pragma warning restore 162
    }
}