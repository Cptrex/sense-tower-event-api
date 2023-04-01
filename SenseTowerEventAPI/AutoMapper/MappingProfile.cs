using AutoMapper;
using JetBrains.Annotations;
using SenseTowerEventAPI.Features.Event.EventCreate;

namespace SenseTowerEventAPI.AutoMapper;

[UsedImplicitly]
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EventCreateCommand, Models.Event>();
    }
}