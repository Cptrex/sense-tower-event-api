using NUnit.Framework;
using SenseTowerEventAPI.Features.Event;
using SenseTowerEventAPI.Features.Event.EventCreate;
using SenseTowerEventAPI.Interfaces;
using Moq;
#pragma warning disable CS8618

namespace SenseTowerEventAPI.UnitTests;

[TestFixture]
public class EventValidatorTests
{
    private EventCreateValidator validator;

    [SetUp]
    public void SetUp()
    {
        var httpClientFactory = new Mock<IHttpClientFactory>();
        IEventValidatorManager validatorManger = new EventValidatorManager(httpClientFactory.Object);

        validator = new EventCreateValidator(validatorManger);
    }

    [Test]
    public void Does_EventCreateCommandModel_Title_Have_letter_s()
    {
        EventCreateCommand cmd = new()
        {
            Id = Guid.NewGuid(),
            Title = "sep",
            Description = "desc",
            EndDate = DateTimeOffset.Now.AddDays(1),
            StartDate = DateTimeOffset.Now,
            ImageId = Guid.NewGuid(),
            SpaceId = Guid.NewGuid()
        };
        var result = validator.Validate(cmd);

        Assert.That(result.IsValid, Is.EqualTo(true));
    }

    [Test]
    public void Does_EventCreateCommandModel_Title_Have_Minimum_1_symbol()
    {
        EventCreateCommand cmd = new()
        {
            Id = Guid.NewGuid(),
            Title = "s",
            Description = "desc",
            EndDate = DateTimeOffset.Now.AddDays(1),
            StartDate = DateTimeOffset.Now,
            ImageId = Guid.NewGuid(),
            SpaceId = Guid.NewGuid()
        };
        var result = validator.Validate(cmd);

        Assert.That(result.IsValid, Is.EqualTo(true));
    }
}