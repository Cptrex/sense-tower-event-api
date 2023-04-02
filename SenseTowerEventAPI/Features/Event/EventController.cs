using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Internship.Common.ScResult;
using SenseTowerEventAPI.Features.Event.EventCreate;
using SenseTowerEventAPI.Features.Event.EventDelete;
using SenseTowerEventAPI.Features.Event.EventGetList;
using SenseTowerEventAPI.Features.Event.EventUpdate;
using SenseTowerEventAPI.Filters;

namespace SenseTowerEventAPI.Features.Event;

[ApiController]
[Authorize]
[Route("[controller]")]
[ServiceFilter(typeof(StValidationFilter))]
public class EventController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Создать мероприятие
    /// </summary>
    /// <param name="cmd">Поля мероприятия</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Возвращает HTTP Response Code</returns>
    /// <remarks> GUID для проверки валидации ImageID и SpaceID 3fa85f64-5717-4562-b3fc-2c963f66afa6</remarks>
    [HttpPost]
    public async Task<ScResult> CreateEvent(EventCreateCommand cmd, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false) return new ScResult { Error = new ScError { Message = "Ошибка передачи данных" } };

        await _mediator.Send(cmd, cancellationToken);

        return new ScResult();
    }

    /// <summary>
    /// Удалить выбранное мероприятия по GUID
    /// </summary>
    /// <param name="eventId">GUID мероприятия</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Возвращает HTTP Response Code</returns>
    [HttpDelete("/{eventId:guid}")]
    public async Task<ScResult> DeleteEvent([FromRoute] Guid eventId, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false) return new ScResult { Error = new ScError { Message = "Возникла ошибка передачи данных" }};

        var cmd = new EventDeleteCommand { Id = eventId };
        await _mediator.Send(cmd, cancellationToken);

        return new ScResult();
    }

    /// <summary>
    /// Изменить информацию выбранного мероприятия по GUID
    /// </summary>
    /// <param name="eventId">GUID мероприятия</param>
    /// <param name="cmd">Поля мероприятия</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Возвращает HTTP Response Code</returns>
    [HttpPut("/{eventId:guid}")]
    public async Task<ScResult> UpdateEventById([FromRoute] Guid eventId, EventUpdateCommand cmd, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false) return new ScResult { Error = new ScError { Message = "Ошибка передачи данных" } };

        await _mediator.Send(cmd, cancellationToken);

        return new ScResult();
    }

    /// <summary>
    /// Получить весь список активных мероприятий
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список всех активных мероприятий</returns>
    [HttpGet("events")]
    public async Task<ScResult<IEnumerable<Models.Event>>> GetEventsList(CancellationToken cancellationToken)
    {
        if (ModelState.IsValid == false) return new ScResult<IEnumerable<Models.Event>>{ Error = new ScError { Message = "Ошибка передачи данных" } };

        var cmd = new EventGetListQuery();
        var eventList = await _mediator.Send(cmd, cancellationToken);

        return new ScResult<IEnumerable<Models.Event>>(eventList);
    }
}