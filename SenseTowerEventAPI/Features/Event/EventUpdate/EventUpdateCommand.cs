using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace SenseTowerEventAPI.Features.Event.EventUpdate;

/// <summary>
/// Модель команды обновления мероприятия
/// </summary>
[SwaggerSchema("Модель команды обновления мероприятия")]
public class EventUpdateCommand : IRequest<Guid>
{
    /// <summary>
    /// Уникальный идентификатор мероприятия
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Название мероприятия
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Дата начала меропрития
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// Дата завершения мероприятия
    /// </summary>
    public DateTime EndDate { get; set; }
    /// <summary>
    /// Описание мероприятия
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Уникальный идентификатор изображения мероприятия
    /// </summary>
    public Guid ImageId { get; set; }
    /// <summary>
    /// Уникальный идентификатор пространства мероприятия
    /// </summary>
    public Guid SpaceId { get; set; }
    /// <summary>
    /// Список билетов мероприятия
    /// </summary>
    public List<Models.Ticket> Tickets { get; set; }

    public EventUpdateCommand(Guid id, string title, DateTime startDate, DateTime endDate,
        string description, Guid imageId, Guid spaceId, List<Models.Ticket> tickets)
    {
        Id = id;
        Title = title;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        ImageId = imageId;
        SpaceId = spaceId;
        Tickets = tickets;
    }
}