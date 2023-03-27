namespace ST.Services.Payment.Utils;

public enum PaymentState
{
    Hold = 0,
    Confirmed = 1,
    Canceled = -1
}