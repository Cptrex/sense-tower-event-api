using SC.Internship.Common.Exceptions;

namespace SenseTowerEventAPI.Extensions;

public sealed class ValidationException : ScException
{
    public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
        : base( "Произошла ошибка!")
        => ErrorsDictionary = errorsDictionary;

    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
}
