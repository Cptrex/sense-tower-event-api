using JetBrains.Annotations;
using SC.Internship.Common.Exceptions;

namespace SenseTowerEventAPI.Extensions;

[UsedImplicitly]
public class StException : ScException
{
    public StException(string? message) : base(message)
    {
    }

    public StException(Exception? exception, string? message) : base(exception, message)
    {
    }
}