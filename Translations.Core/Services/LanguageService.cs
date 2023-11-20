#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.Runtime.CompilerServices;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Translations.Core.Common.Data.Interfaces;
using Translations.Core.Common.Services.Bases;
using Translations.Core.Entities;
using Translations.Shared.Outputs;

namespace Translations.Core.Services;

public class LanguageService : BaseService<Language>, ILanguageService
{
    private static string GetLogMessage(string message, [CallerMemberName] string callerName = null)
    {
        return $"[{nameof(LanguageService)}.{callerName}] - {message}";
    }
    private readonly IRepositoryAsync<Engine> _engineRepository;


    public LanguageService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _engineRepository = UnitOfWork.RepositoryAsync<Engine>();
    }

    private IQueryable<Language> Languages => Repository.Queryable().Include(x => x.Engines).ThenInclude(x => x.Engine);

    private IQueryable<Engine> Engines =>
        _engineRepository.Queryable().Include(x => x.Languages).ThenInclude(x => x.Language);

    public async Task<List<T>> GetLanguagesAsync<T>() where T : LanguageOutput
    {
        var languages = await Engines.Where(x => x.Enabled).SelectMany(x => x.Languages).Select(x => x.Language).Distinct()
            .ProjectTo<T>(ProjectionMapping)
            .ToListAsync();

        return languages;
    }

    public Task<List<T>> GetAllLanguagesAsync<T>() where T : LanguageOutput
    {
        return Languages.ProjectTo<T>(ProjectionMapping).ToListAsync();
    }

    public Task<T> GetLanguageAsync<T>(string languageId) where T : LanguageOutput
    {
        return Languages.Where(x => x.Id == languageId).ProjectTo<T>(ProjectionMapping).FirstOrDefaultAsync();
    }
}