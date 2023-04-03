using Newtonsoft.Json;
using SC.Internship.Common.ScResult;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event;

public class EventValidatorManager : IEventValidatorManager
{
    private readonly HttpClient _httpImageServiceClient;
    private readonly HttpClient _httpSpaceServiceClient;

    public EventValidatorManager(IHttpClientFactory httpFactory)
    {
        _httpSpaceServiceClient = httpFactory.CreateClient("spaceService");
        _httpImageServiceClient = httpFactory.CreateClient("imageService");
    }

    public async Task<bool> IsImageIdExist(Guid imageId)
    {
        var response = await _httpImageServiceClient.GetAsync($"{_httpImageServiceClient.BaseAddress}images/{imageId}");

        var responseParsed = JsonConvert.DeserializeObject<ScResult<bool>>(await response.Content.ReadAsStringAsync());
        
        Console.WriteLine("resImage" + responseParsed);

        return true;
    }

    public async Task<bool> IsSpaceIdExist(Guid spaceId)
    {
        var response = await _httpSpaceServiceClient.GetAsync($"{_httpSpaceServiceClient.BaseAddress}spaces/{spaceId}");

        var responseParsed = JsonConvert.DeserializeObject<ScResult<bool>>(await response.Content.ReadAsStringAsync());

        Console.WriteLine("resSpace" + responseParsed);

        return true;
    }
}