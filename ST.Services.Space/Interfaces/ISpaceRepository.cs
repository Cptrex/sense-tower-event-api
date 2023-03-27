﻿namespace ST.Services.Space.Interfaces;

public interface ISpaceRepository
{
    public Task<bool> DeleteSpaceId(Guid spaceId, CancellationToken cancellationToken);
    // ReSharper disable once UnusedMemberInSuper.Global
    public Task RemoveEventByUsedSpace(Guid spaceId, CancellationToken cancellationToken);
}