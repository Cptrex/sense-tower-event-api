using JetBrains.Annotations;
using ST.Services.Payment.Utils;
// ReSharper disable UnusedMember.Global

namespace ST.Services.Payment.Models;

[UsedImplicitly]
public class PaymentTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public PaymentState State { get; set; } = PaymentState.Hold;
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    public DateTime DateConfirmation { get; set; } = DateTime.MinValue;
    public DateTime DateCancellation { get; set; } = DateTime.MinValue;
    public string? Description { get; set; } = string.Empty;
}