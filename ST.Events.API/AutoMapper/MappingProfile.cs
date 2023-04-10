using AutoMapper;
using JetBrains.Annotations;
using ST.Events.API.Features.Event.EventCreate;

namespace ST.Events.API.AutoMapper;

[UsedImplicitly]
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EventCreateCommand, Models.Event>();
    }
}