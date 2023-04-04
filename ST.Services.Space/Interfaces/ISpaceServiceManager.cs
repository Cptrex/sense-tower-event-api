namespace ST.Services.Space.Interfaces;

public interface ISpaceServiceManager
{
    public bool DeleteSpaceId(Guid spaceId, CancellationToken cancellationToken);
    // ReSharper disable once UnusedMemberInSuper.Global
    public void RemoveEventByUsedSpace(Guid spaceId, CancellationToken cancellationToken);
}