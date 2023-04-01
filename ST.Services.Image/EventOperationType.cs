using System.Diagnostics.CodeAnalysis;

namespace ST.Services.Image;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum EventOperationType
{
    SpaceDeleteEvent = 1,
    ImageDeleteEvent = 2,
    EventDeleteEvent = 3
}