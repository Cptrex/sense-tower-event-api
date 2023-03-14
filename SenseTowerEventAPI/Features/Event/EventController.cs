using MediatR;
using Microsoft.AspNetCore.Mvc;
using SenseTowerEventAPI.Features.Event.EventCreate;
using SenseTowerEventAPI.Features.Event.EventDelete;
using SenseTowerEventAPI.Features.Event.EventRead;
using SenseTowerEventAPI.Features.Event.EventUpdate;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event
{
    [ApiController]
    [Route("api/v1/events")]
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
        public async Task<IActionResult> CreateEvent(EventCreateCommand cmd)
        {
            var validationResult = await _validator.Validate(cmd);

            if(validationResult.IsValid == false)
            {
                var validationFailure = validationResult.Errors.FirstOrDefault(e => e.PropertyName == "SpaceID" ||  e.PropertyName == "ImageID" || e.PropertyName =="StartDate" 
                        || e.PropertyName == "EndDate");

                if(validationFailure != null) return BadRequest($"{validationFailure.ErrorMessage}");
            }

            await _mediator.Send(cmd);

            return Ok();
        }

        /// <summary>
        /// Удалить выбранное мероприятия по GUID
        /// </summary>
        /// <param name="eventId">GUID мероприятия</param>
        /// <returns>Возвращает HTTP Response Code</returns>
        [HttpDelete("delete/{eventId:guid}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] Guid eventId)
        {
            EventDeleteCommand cmd = new EventDeleteCommand { ID = eventId };
            await _mediator.Send(cmd);
            return Ok();
        }

        /// <summary>
        /// Изменить информацию выбранного мероприятия по GUID
        /// </summary>
        /// <param name="eventId">GUID мероприятия</param>
        /// <param name="cmd">Поля мероприятия</param>
        /// <returns>Возвращает HTTP Response Code</returns>
        [HttpPut("update/{eventId:guid}")]
        public async Task<IActionResult> UpdateEventById([FromRoute] Guid eventId, EventUpdateCommand cmd)
        {
            await _mediator.Send(cmd);
            return Ok();
        }

        /// <summary>
        /// Получить весь список активных мероприятий
        /// </summary>
        /// <returns>Список всех активных мероприятий</returns>
        [HttpGet]
        public async Task<IActionResult> GetEventsList()
        {
            EventGetListQuery cmd = new EventGetListQuery();
            var eventList = await _mediator.Send(cmd);
            return Ok(eventList);
        }
    }
}