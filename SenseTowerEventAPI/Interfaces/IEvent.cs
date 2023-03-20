using JetBrains.Annotations;

namespace SenseTowerEventAPI.Interfaces;

/// <summary>
/// Интерфейс мероприятия
/// </summary>
[UsedImplicitly]
public interface IEvent
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
}