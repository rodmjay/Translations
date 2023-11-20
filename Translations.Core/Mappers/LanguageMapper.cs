#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using AutoMapper;
using Translations.Core.Entities;
using Translations.Shared.Outputs;

namespace Translations.Core.Mappers;

public class LanguageMapper : Profile
{
    public LanguageMapper()
    {
        CreateMap<Language, LanguageOutput>().IncludeAllDerived();

        CreateMap<Language, LanguagesWithEnginesOutput>()
            .ForMember(x => x.Engines, opt => opt.MapFrom(x => x.Engines.Select(a => a.Engine)))
            .IncludeAllDerived();
    }
}