using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventGetList;

public class EventGetListQuery : IRequest<List<IEvent>>
{ 
}