namespace ST.Services.Image.Interfaces;

public interface IImageRepository
{
    public Task<bool> DeleteImageById(Guid imageId, CancellationToken cancellationToken);
    // ReSharper disable once UnusedMemberInSuper.Global
    public Task ReplaceEventImageByDefault(Guid imageId, CancellationToken cancellationToken);
}