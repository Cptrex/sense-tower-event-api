using JetBrains.Annotations;

namespace SenseTowerEventAPI.Exceptions;

[UsedImplicitly]
public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(int userId)
        : base($"The user with the identifier {userId} was not found.")
    {
    }
}