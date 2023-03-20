using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Internship.Common.ScResult;
using SenseTowerEventAPI.Features.Event.EventCreate;
using SenseTowerEventAPI.Features.Event.EventDelete;
using SenseTowerEventAPI.Features.Event.EventGetList;
using SenseTowerEventAPI.Features.Event.EventUpdate;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event;

[ApiController]
[Authorize]
[Route("[controller]")]
public class EventController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IEventValidatorBehavior _validator;

    public EventController(IMediator mediator, IEventValidatorBehavior validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    /// <summary>
    /// Создать мероприятие
    /// </summary>
    /// <param name="cmd">Поля мероприятия</param>
    /// <returns>Возвращает HTTP Response Code</returns>
    /// <remarks> GUID для проверки валидации ImageID и SpaceID 3fa85f64-5717-4562-b3fc-2c963f66afa6</remarks>
    [HttpPost("create")]
    public async Task<ScResult> CreateEvent(EventCreateCommand cmd)
    {
        var result = _validator.Validate(cmd);
        await _mediator.Send(cmd);

        return new ScResult<ValidationResult>(result);
    }

    /// <summary>
    /// Удалить выбранное мероприятия по GUID
    /// </summary>
    /// <param name="eventId">GUID мероприятия</param>
    /// <returns>Возвращает HTTP Response Code</returns>
    [HttpDelete("delete/{eventId:guid}")]
    public async Task<ScResult> DeleteEvent([FromRoute] Guid eventId)
    {
        var cmd = new EventDeleteCommand { Id = eventId };
        await _mediator.Send(cmd);

        return new ScResult();
    }

    /// <summary>
    /// Изменить информацию выбранного мероприятия по GUID
    /// </summary>
    /// <param name="eventId">GUID мероприятия</param>
    /// <param name="cmd">Поля мероприятия</param>
    /// <returns>Возвращает HTTP Response Code</returns>
    [HttpPut("update/{eventId:guid}")]
    public async Task<ScResult> UpdateEventById([FromRoute] Guid eventId, EventUpdateCommand cmd)
    {
        await _mediator.Send(cmd);

        return new ScResult();
    }

    /// <summary>
    /// Получить весь список активных мероприятий
    /// </summary>
    /// <returns>Список всех активных мероприятий</returns>
    [HttpGet]
    public async Task<ScResult<IEnumerable<IEvent>>> GetEventsList()
    {
        var cmd = new EventGetListQuery();
        var eventList = await _mediator.Send(cmd);

        return new ScResult<IEnumerable<IEvent>>(eventList);
    }
}