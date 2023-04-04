using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Ticket.AddTicket;

/// <summary>
/// Модель команды добавления бесплатного билета
/// </summary>
[UsedImplicitly]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class AddTicketCommand : ICommand<Guid>
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid Owner { get; set; }
    public int PlaceNumber { get; set; }
    public decimal Price { get; set; }
}