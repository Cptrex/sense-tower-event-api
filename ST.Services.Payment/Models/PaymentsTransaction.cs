using JetBrains.Annotations;
using ST.Services.Payment.Enumerics;
// ReSharper disable UnusedMember.Global

namespace ST.Services.Payment.Models;

[UsedImplicitly]
public class PaymentsTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public PaymentState State { get; set; } = PaymentState.Hold;
    public DateTimeOffset DateCreation { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset DateConfirmation { get; set; } = DateTimeOffset.MinValue;
    public DateTimeOffset DateCancellation { get; set; } = DateTimeOffset.MinValue;
    public string? Description { get; set; } = string.Empty;
}