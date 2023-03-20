using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Internship.Common.ScResult;
using SenseTowerEventAPI.Features.Ticket.AddFreeTicket;
using SenseTowerEventAPI.Features.Ticket.CheckTicketUserExist;
using SenseTowerEventAPI.Features.Ticket.GiveTicketUser;

namespace SenseTowerEventAPI.Features.Ticket;

[ApiController]
[Authorize]
[Route("[controller]")]
public class TicketController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Выдать бесплатный билет пользователю
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("give-free-ticket")]
    public async Task<ScResult<Guid>> GiveFreeTicketUser(GiveTicketUserCommand cmd)
    {
        var result = await _mediator.Send(cmd);

        return new ScResult<Guid>(result);
    }

    /// <summary>
    /// Проверить есть ли билет у пользователя на данное мероприятие
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("check-ticket-exist")]
    public async Task<ScResult<bool>> CheckUserTicketExist(CheckTicketUserExistQuery cmd)
    {
        var searchResult = await _mediator.Send(cmd);
        
        return await Task.FromResult(new ScResult<bool>(searchResult));
    }

    /// <summary>
    /// Добавить беслпатный билет на мероприятие
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("add-free-ticket")]
    public async Task<ScResult<Guid>> AddFreeTicketEvent(AddFreeTicketCommand cmd)
    {
        var result = await _mediator.Send(cmd);

        return await Task.FromResult(new ScResult<Guid>(result));
    }
}