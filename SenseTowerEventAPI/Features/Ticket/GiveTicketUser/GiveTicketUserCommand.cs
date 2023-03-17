﻿using MediatR;
using SenseTowerEventAPI.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SenseTowerEventAPI.Features.Ticket.GiveTicketUser;

/// <summary>
/// Модель команды выдачи билета пользователю на мероприятие
/// </summary>
[SwaggerSchema("Модель команды выдачи билета пользователю на мероприятие")]
public class GiveTicketUserCommand : IRequest<Guid>, ITicket, IEntity
{
    /// <summary>
    /// Уникальный идентификатор билета
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Уникальный идентификатор мероприятия 
    /// </summary>
    public Guid EventId { get; set; }
    /// <summary>
    /// Владелец билета
    /// </summary>
    public Guid Owner { get; set; }
    /// <summary>
    /// Место мероприятия
    /// </summary>
    public string? Place { get; set; }

    public GiveTicketUserCommand(Guid id, Guid eventId, Guid owner, string? place)
    {
        Id = id;
        EventId = eventId;
        Owner = owner;
        Place = place;
    }
}