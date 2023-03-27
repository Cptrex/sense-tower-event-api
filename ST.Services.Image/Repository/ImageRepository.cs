using ST.Services.Image.Interfaces;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ST.Services.Image.Repository;

public class ImageRepository : IImageRepository
{
    private readonly IImageSingleton _imageInstance;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ImageRepository(HttpClient client, IImageSingleton imageInstance, IConfiguration endpointOptions)
    {
        _httpClient = client;
        _imageInstance = imageInstance;

        _configuration = endpointOptions;
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
        var eventServiceUri = _configuration["ServiceEndpoints:EventServiceURL"];

        content.Headers.Add("Authorization", $"{JwtBearerDefaults.AuthenticationScheme} {eventServiceUri}");

        await _httpClient.PutAsync(new Uri($"{eventServiceUri}/event?{imageId}"), content, cancellationToken);
    }
}