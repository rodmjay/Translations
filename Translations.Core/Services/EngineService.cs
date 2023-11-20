using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Translations.Core.Common.Services.Bases;
using Translations.Core.Entities;
using Translations.Shared.Outputs;

namespace Translations.Core.Services
{
    public class EngineService : BaseService<Engine>, IEngineService
    {
        public EngineService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        private IQueryable<Engine> Engines =>
            Repository.Queryable();

        public Task<List<T>> GetEngines<T>() where T : EngineOutput
        {
            return Engines.ProjectTo<T>(ProjectionMapping).ToListAsync();
        }
    }
}
