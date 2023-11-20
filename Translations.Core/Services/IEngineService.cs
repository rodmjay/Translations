using Translations.Core.Common.Services.Interfaces;
using Translations.Core.Entities;
using Translations.Shared.Outputs;

namespace Translations.Core.Services
{
    public interface IEngineService : IService<Engine>
    {
        Task<List<T>> GetEngines<T>() where T : EngineOutput;
    }
}
