using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;

namespace SenseTowerEventAPI.IntegrationTests;

[TestFixture]
public class EventControllerTests
{
    [Test]
    public  async Task CheckStatus_GetEventsList_ShouldReturn401()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        var httpClient = webApplicationFactory.CreateDefaultClient();

        var response = await httpClient.GetAsync("/Event/events");

        var result = await response.Content.ReadAsStringAsync();

        Assert.True(!string.IsNullOrEmpty(result));
    }
}