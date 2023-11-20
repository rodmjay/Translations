#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Microsoft.AspNetCore.Mvc;
using Translations.Common.Bases;
using Translations.Core.Managers;
using Translations.Shared.Interfaces;
using Translations.Shared.Options;
using Translations.Shared.Outputs;

namespace Translations.Controllers;

public class TranslateController : BaseController, ITranslateController
{
   
    private readonly TranslationManager _translationManager;

    public TranslateController(IServiceProvider serviceProvider, 
        TranslationManager translationManager) : base(serviceProvider)
    {
        _translationManager = translationManager;
    }
    
    [HttpPost]
    public async Task<List<PhraseOutput>> TranslatePhrases(
        [FromBody] PhraseBulkCreateOptions input)
    {

        var result = await _translationManager.Translate<PhraseOutput>(input);
        
        return result;
    }
}