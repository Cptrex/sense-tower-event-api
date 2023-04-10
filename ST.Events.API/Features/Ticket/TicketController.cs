using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Internship.Common.ScResult;
using ST.Events.API.Features.Ticket.AddTicket;
using ST.Events.API.Features.Ticket.CheckTicketUserExist;
using ST.Events.API.Features.Ticket.GiveTicketUser;
using ST.Events.API.Filters;

namespace ST.Events.API.Features.Ticket;

[ApiController]
[Authorize]
[Route("[controller]")]
[ServiceFilter(typeof(StValidationFilter))]
public class TicketController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Выдать  билет пользователю
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ScResult<Guid>> GiveTicketUser(GiveTicketUserCommand cmd)
    {
        if (ModelState.IsValid == false) return new ScResult<Guid> { Error = new ScError { Message = "Ошибка передачи данных" } };

        var result = await _mediator.Send(cmd);

        return new ScResult<Guid>(result);
    }

    /// <summary>
    /// Проверить есть ли билет у пользователя на данное мероприятие
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ScResult<bool>> CheckUserTicketExist(CheckTicketUserExistQuery cmd)
    {
        if (ModelState.IsValid == false) return new ScResult<bool> { Error = new ScError { Message = "Ошибка передачи данных" } };

        var searchResult = await _mediator.Send(cmd);
        
        return new ScResult<bool>(searchResult);
    }

    /// <summary>
    /// Добавить билет на мероприятие
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ScResult<Guid>> AddTicketEvent(AddTicketCommand cmd)
    {
        if (ModelState.IsValid == false) return new ScResult<Guid> { Error = new ScError { Message = "Ошибка передачи данных" } };

        var result = await _mediator.Send(cmd);

        return new ScResult<Guid>(result);
    }
}