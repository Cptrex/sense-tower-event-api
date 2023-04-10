using JetBrains.Annotations;
using SC.Internship.Common.Exceptions;

namespace ST.Events.API.Exceptions;

[UsedImplicitly]
public sealed class ValidationException : ScException
{
    public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
        : base( "Обнаружена одна или несколько ошибок валидации")
        => ErrorsDictionary = errorsDictionary;

    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
}