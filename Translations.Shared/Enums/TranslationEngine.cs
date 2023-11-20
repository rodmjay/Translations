#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.ComponentModel;

namespace Translations.Shared.Enums;

public enum TranslationEngine : int
{
    [Description("Google Cloud Translate")]
    Google = 1,

    [Description("Azure Translator by Microsoft")]
    Azure = 2,

    [Description("Amazon Translate")]
    Amazon = 3,

    [Description("DeepL")]
    DeepL = 4
}