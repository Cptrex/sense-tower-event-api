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
        _httpImageServiceClient = httpFactory.CreateClient("imageService");
        _httpSpaceServiceClient = httpFactory.CreateClient("spaceService");
    }

    public bool IsImageIdExist(Guid imageId)
    {
        var response = _httpImageServiceClient.GetAsync($"{_httpImageServiceClient.BaseAddress}images/{imageId}").Result;

        var responseParsed = JsonConvert.DeserializeObject<ScResult<bool>>(response.Content.ReadAsStringAsync().Result);

        return responseParsed!.Result;
    }

    public bool IsSpaceIdExist(Guid spaceId)
    {
        var response = _httpSpaceServiceClient.GetAsync($"{_httpSpaceServiceClient}spaces/{spaceId}").Result;

        var responseParsed = JsonConvert.DeserializeObject<ScResult<bool>>(response.Content.ReadAsStringAsync().Result);

        return responseParsed!.Result;
    }
}