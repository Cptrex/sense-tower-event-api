using MediatR;

namespace SenseTowerEventAPI.Features.Event.EventGetList;

public class EventGetListQuery : IRequest<List<Models.Event>>
{ 
}