#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Shared.Outputs;

namespace Translations.Core.Errors;

public class TranslationErrorDescriber
{
    public virtual Error LanguageDoesntExistInApplication(string language, string application)
    {
        return new Error
        {
            Code = nameof(TranslationDoesntExist),
            Description = $"Language '{language}' doesn't exist in application '{application}"
        };
    }

    public virtual Error TranslationDoesntExist(string text)
    {
        return new Error
        {
            Code = nameof(TranslationDoesntExist),
            Description = $"Translation '{text}' doesn't exist"
        };
    }

    public virtual Error UnableToDeleteTranslation(string text)
    {
        return new Error
        {
            Code = nameof(UnableToDeleteTranslation),
            Description = $"Unable to delete Translation '{text}'"
        };
    }

    public virtual Error UnableToCreateTranslation()
    {
        return new Error
        {
            Code = nameof(UnableToCreateTranslation),
            Description = "Unable to create Translation"
        };
    }

    public virtual Error UnableToUpdateTranslation(string text)
    {
        return new Error
        {
            Code = nameof(UnableToUpdateTranslation),
            Description = $"Unable to update Translation '{text}'"
        };
    }
}