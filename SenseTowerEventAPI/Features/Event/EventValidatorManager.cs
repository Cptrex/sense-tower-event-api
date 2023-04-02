using Newtonsoft.Json;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event;

public class EventValidatorManager : IEventValidatorManager
{
    private readonly HttpClient _httpImageServiceClient;
    private readonly HttpClient _httpSpaceServiceClient;
    private readonly IConfiguration _configuration;

    public EventValidatorManager(IHttpClientFactory httpFactory, IConfiguration configuration)
    {
        _configuration = configuration;
        _httpImageServiceClient = httpFactory.CreateClient("imageService");
        _httpSpaceServiceClient = httpFactory.CreateClient("spaceService");
    }

    public bool IsImageIdExist(Guid imageId)
    {
        var response = _httpImageServiceClient.GetStringAsync(new Uri($"{_configuration["ImageService:URL"]}/images/{imageId}")).Result;

        var isImageExist = JsonConvert.DeserializeObject<bool>(response);
        
        return isImageExist;
    }

    public bool IsSpaceIdExist(Guid spaceId)
    {
        var response = _httpSpaceServiceClient.GetStringAsync(new Uri($"{_configuration["SpaceService:URL"]}/spaces/{spaceId}")).Result;

        var isSpaceExist = JsonConvert.DeserializeObject<bool>(response);

        return isSpaceExist;
    }
}