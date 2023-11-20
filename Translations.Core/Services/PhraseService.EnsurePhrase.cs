#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Microsoft.EntityFrameworkCore;
using Translations.Core.Entities;
using Translations.Core.Models;
using Translations.Shared.Outputs;

namespace Translations.Core.Services;

public partial class PhraseService
{
    public async Task<Result> EnsurePhrase(string text)
    {
        var phrase = await Phrases.Where(x => x.Text == text).FirstOrDefaultAsync();
        if (phrase != null) return Result.Success(phrase.Id);

        phrase = new Phrase()
        {
            Text = text
        };

        var records = Repository.Insert(phrase, true);
        if (records > 0)
        {
            return Result.Success(phrase.Id);
        }

        return Result.Failed();
    }
    

    public async Task<EnsurePhrasesResult> EnsurePhrases(string[] input)
    {
        var texts = input
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x=>x.Trim())
            .ToArray();

        var retVal = new EnsurePhrasesResult()
        {
            PhrasesRequested = texts.Length,
        };

        var existingPhrases = await Phrases.Where(x => texts.Contains(x.Text)).ToListAsync();

        retVal.ExistingPhrases = existingPhrases.Count;
        
        foreach (var text in texts)
        {
            if (existingPhrases.Any(x => x.Text == text)) continue;

            var phrase = new Phrase()
            {
                Text = text
            };

            Repository.Insert(phrase);
        }

        retVal.PhrasesAdded = Repository.Commit();

        retVal.Phrases = await Phrases.Where(x => texts.Contains(x.Text)).Select(x=>x.Id).ToArrayAsync();

        return retVal;
    }
}