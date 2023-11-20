using AutoMapper;
using Translations.Core.Entities;
using Translations.Shared.Outputs;

namespace Translations.Core.Mappers
{
    public class EngineMapper : Profile
    {
        public EngineMapper()
        {
            CreateMap<Engine, EngineOutput>()
                .IncludeAllDerived();

            CreateMap<Engine, EngineWithLanguagesOutput>()
                .ForMember(x => x.Languages, opt => opt.MapFrom(x => x.Languages.Select(a => a.Language)));
        }
    }
}
