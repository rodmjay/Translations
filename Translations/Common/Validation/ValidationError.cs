#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Newtonsoft.Json;

namespace Translations.Common.Validation;

public class ValidationError
{
    public ValidationError(string field, string message)
    {
        Field = field != string.Empty ? field : null;
        Message = message;
    }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Field { get; }

    public string Message { get; }
}