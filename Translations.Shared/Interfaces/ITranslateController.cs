#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Shared.Options;
using Translations.Shared.Outputs;

namespace Translations.Shared.Interfaces;

public interface ITranslateController
{
    Task<List<PhraseOutput>> TranslatePhrases(
         PhraseBulkCreateOptions input);
}