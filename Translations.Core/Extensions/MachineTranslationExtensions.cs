#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Core.Entities;

namespace Translations.Core.Extensions;

public static class MachineTranslationExtensions
{
    public static bool HasLanguage(this IEnumerable<MachineTranslation> translations, string language)
    {
        return translations.Any(x => x.LanguageId == language);
    }
}