using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SenseTowerEventAPI.Features.Event;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.Models;

namespace SenseTowerEventAPI.UnitTests;

[TestFixture]
public class EventValidatorRepositoryTests
{
#pragma warning disable CS8618
    private IEventSingleton _eventInstance;
    private IEventValidatorManager _eventValidatorRepository;
#pragma warning restore CS8618

    [SetUp]
    public void SetUp()
    {
        _eventInstance = new EventSingleton();
        IHttpClientFactory iHttpClientFactory = null;
        IConfiguration iconfiguration = null;
        //_eventValidatorRepository = new EventValidatorManager(iHttpClientFactory, iconfiguration);

       /* _eventInstance.Images = new List<Guid>
        {
            new("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            new("4fa85f64-5717-4562-b3fc-2c963f66afa6"),
            new("5fa85f64-5717-4562-b3fc-2c963f66afa6")
        };

        _eventInstance.Spaces = new List<Guid>
        {
            new("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            new("4fa85f64-5717-4562-b3fc-2c963f66afa6"),
            new("5fa85f64-5717-4562-b3fc-2c963f66afa6")
        };*/
    }

    [Test]
    public void IsImageGuidExist_Should_Return_False()
    {
        var findImageGuid = new Guid("22a85f64-5717-4562-b3fc-2c963f66afa6");
        var result = _eventValidatorRepository.IsImageIdExist(findImageGuid);
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsImageGuidExist_Should_Return_True()
    {
        var findImageGuid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var result = _eventValidatorRepository.IsImageIdExist(findImageGuid);
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsSpaceGuidExist_Should_Return_True()
    {
        var findSpaceGuid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var result = _eventValidatorRepository.IsSpaceIdExist(findSpaceGuid);
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsSpaceGuidExist_Should_Return_False()
    {
        var findSpaceGuid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var result = _eventValidatorRepository.IsSpaceIdExist(findSpaceGuid);
        Assert.That(result, Is.True);
    }
}