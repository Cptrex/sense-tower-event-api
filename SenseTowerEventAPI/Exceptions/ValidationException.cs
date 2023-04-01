using JetBrains.Annotations;
using SC.Internship.Common.Exceptions;

namespace SenseTowerEventAPI.Exceptions;

[UsedImplicitly]
public sealed class ValidationException : ScException
{
    public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
        : base("Произошла ошибка!")
        => ErrorsDictionary = errorsDictionary;

    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
}