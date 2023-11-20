#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using AutoMapper;
using Translations.Core.Entities;
using Translations.Shared.Outputs;

namespace Translations.Core.Mappers;

public class PhraseProjections : Profile
{
    public PhraseProjections()
    {
        CreateMap<Phrase, PhraseOutput>()
            .ForMember(x=>x.MachineTranslations, opt=>opt.MapFrom(x=>x.MachineTranslations))
            .IncludeAllDerived();
    }
}