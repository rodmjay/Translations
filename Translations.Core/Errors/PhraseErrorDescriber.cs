#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Shared.Outputs;

namespace Translations.Core.Errors;

public class PhraseErrorDescriber
{
    public virtual Error PhraseDoesntExist(int id)
    {
        return new Error
        {
            Code = nameof(PhraseDoesntExist),
            Description = $"Phrase '{id}' doesn't exist"
        };
    }

    public virtual Error UnableToDeletePhrase(int id)
    {
        return new Error
        {
            Code = nameof(UnableToDeletePhrase),
            Description = $"Unable to delete phrase '{id}'"
        };
    }

    public virtual Error UnableToCreatePhrase()
    {
        return new Error
        {
            Code = nameof(UnableToCreatePhrase),
            Description = "Unable to create phrase"
        };
    }

    public virtual Error UnableToUpdatePhrase(int id)
    {
        return new Error
        {
            Code = nameof(UnableToUpdatePhrase),
            Description = $"Unable to update phrase '{id}'"
        };
    }
}