using MediatR;

namespace ST.Events.API.Features.Event.EventGetList;

public class EventGetListQuery : IRequest<List<Models.Event>>
{ 
}