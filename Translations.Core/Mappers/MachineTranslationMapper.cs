#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using AutoMapper;
using Translations.Core.Entities;
using Translations.Shared.Options;

namespace Translations.Core.Mappers;

public class MachineTranslationMapper : Profile
{
    public MachineTranslationMapper()
    {
        CreateMap<MachineTranslation, MachineTranslationOutput>()
            .ForMember(x => x.Engine, opt => opt.MapFrom(x => x.Engine.Name));

    }
}