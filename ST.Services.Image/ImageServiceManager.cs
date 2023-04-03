using ST.Services.Image.Interfaces;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ST.Services.Image;

public class ImageServiceManager : IImageServiceManager
{
    private readonly IImageSingleton _imageInstance;
    private readonly HttpClient _httpClient;

    public ImageServiceManager(HttpClient client, IImageSingleton imageInstance)
    {
        _httpClient = client;
        _imageInstance = imageInstance;
    }

    public async Task<bool> DeleteImageById(Guid imageId, CancellationToken cancellationToken)
    {
        var result = _imageInstance.Images.RemoveAll(i => i == imageId);

        if (result <= 0) return false;

        await ReplaceEventImageByDefault(imageId, cancellationToken);

        return true;
    }

    public async Task ReplaceEventImageByDefault(Guid imageId, CancellationToken cancellationToken)
    {
        HttpContent content = new StringContent(null!, Encoding.UTF8, "application/json");
        var eventServiceUri = Environment.GetEnvironmentVariable("ServiceEndpoints__EventService__URL");

        content.Headers.Add("Authorization", $"{JwtBearerDefaults.AuthenticationScheme} {eventServiceUri}");

        await _httpClient.PutAsync(new Uri($"{eventServiceUri}/event?{imageId}"), content, cancellationToken);
    }
}