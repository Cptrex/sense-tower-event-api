// ReSharper disable IdentifierTypo
namespace ST.Services.Payment.Enumerics;

public enum PaymentState
{
    Hold = 0,
    Confirmed = 1,
    Canceled = -1
}