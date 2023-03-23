using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SenseTowerEventAPI.Features.Ticket.CheckTicketUserExist;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.Models.Context;

namespace SenseTowerEventAPI.Tests;

[TestFixture]
public class CheckTicketUserExistHandlerTests
{
    private readonly Mock<IEventSingleton> _eventInstanceMock;
    private readonly Mock<IOptions<EventContext>> _optionsMock;

    public CheckTicketUserExistHandlerTests()
    {
        _eventInstanceMock = new Mock<IEventSingleton>();
        _optionsMock = new Mock<IOptions<EventContext>>();
    }
    //TODO: Не работает. Разобраться как мокать Монго ДБ контекст для тестирования
    [Test]
    public async Task Check_Is_User_Contains_Ticket_to_Event()
    {
        var command = new CheckTicketUserExistQuery();

        var handler = new CheckTicketUserExistHandler(_eventInstanceMock.Object, _optionsMock.Object);

        var result = await handler.Handle(command, default);

        Assert.That(result, Is.True);
    }
}