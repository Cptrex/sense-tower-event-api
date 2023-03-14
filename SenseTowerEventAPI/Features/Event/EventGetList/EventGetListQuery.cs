using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventRead
{
    public class EventGetListQuery : IRequest<List<IEvent>>
    { 
    }
}