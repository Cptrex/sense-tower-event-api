using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ST.Services.Space.Interfaces;

namespace ST.Services.Space.Repository;

public class SpaceRepository : ISpaceRepository
{
    private readonly ISpaceSingleton _spaceInstance;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public SpaceRepository(HttpClient client, ISpaceSingleton spaceInstance,IConfiguration config)
    {
        _httpClient = client;
        _spaceInstance = spaceInstance;
        _configuration = config;
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
        HttpContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        var eventServiceUri = _configuration["ServiceEndpoints:EventServiceURL"];
        var authToken = _configuration["ServiceEndpoints:TokenAuthorization"];

        content.Headers.Add("Authorization", $"{JwtBearerDefaults.AuthenticationScheme} {authToken}");

        await _httpClient.DeleteAsync(new Uri($"{eventServiceUri}/event?{spaceId}"), cancellationToken);
    }
}