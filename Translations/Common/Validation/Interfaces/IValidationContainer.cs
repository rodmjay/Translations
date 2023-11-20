#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

namespace Translations.Common.Validation.Interfaces;

public interface IValidationContainer
{
    IDictionary<string, IList<string>> ValidationErrors { get; }
    bool IsValid { get; }
}

public interface IValidationContainer<out T> : IValidationContainer
{
    T Entity { get; }
}