using ST.Services.Space.Interfaces;

namespace ST.Services.Space;

public class SpaceServiceManager : ISpaceServiceManager
{
    private readonly ISpaceSingleton _spaceInstance;
    private readonly HttpClient _httpClient;

    public SpaceServiceManager(HttpClient client, ISpaceSingleton spaceInstance)
    {
        _httpClient = client;
        _spaceInstance = spaceInstance;
    }

    public async Task<bool> DeleteSpaceId(Guid spaceId, CancellationToken cancellationToken)
    {
        var result = _spaceInstance.Spaces.RemoveAll(i => i == spaceId);

        if (result <= 0) return false;

        await RemoveEventByUsedSpace(spaceId, cancellationToken);

        return true;
    }

    public async Task RemoveEventByUsedSpace(Guid spaceId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/event?{spaceId}", cancellationToken);
    }
}