// ReSharper disable IdentifierTypo

using JetBrains.Annotations;

namespace SenseTowerEventAPI.Enumerics;

public enum PaymentState
{
    [UsedImplicitly] Hold = 0,
    Confirmed = 1,
    Canceled = -1
}