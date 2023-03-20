using NUnit.Framework;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.Models;
using SenseTowerEventAPI.Repository.EventRepository;

namespace SenseTowerEventAPI.Tests
{
    [TestFixture]
    public class EventValidatorRepositoryTests
    {
#pragma warning disable CS8618
        private IEventSingleton _eventInstance;
        private IEventValidatorRepository _eventValidatorRepository;
#pragma warning restore CS8618

        [SetUp]
        public void SetUp()
        {
            _eventInstance = new EventSingleton();
            _eventValidatorRepository = new EventValidatorRepository();
        }

        [Test]
        public void Test_Is_ImageGuid_Exist()
        {
            _eventInstance.Images = new List<Guid>
            {
                new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                new Guid("4fa85f64-5717-4562-b3fc-2c963f66afa6"),
                new Guid("5fa85f64-5717-4562-b3fc-2c963f66afa6")
            };
            var findImageGuid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
            var result = _eventValidatorRepository.IsImageIdExist(_eventInstance, findImageGuid);
            Assert.That(result, Is.True);

            findImageGuid = new Guid("22a85f64-5717-4562-b3fc-2c963f66afa6");
            result = _eventValidatorRepository.IsImageIdExist(_eventInstance, findImageGuid);
            Assert.That(result, Is.False);
        }

        [Test]
        public void Test_Is_SpaceGuid_Exist()
        {
            _eventInstance.Spaces = new List<Guid>
            {
                new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                new Guid("4fa85f64-5717-4562-b3fc-2c963f66afa6"),
                new Guid("5fa85f64-5717-4562-b3fc-2c963f66afa6")
            };
            var findImageGuid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
            var result = _eventValidatorRepository.IsSpaceIdExist(_eventInstance, findImageGuid);
            Assert.That(result, Is.True);

            findImageGuid = new Guid("12a85f64-5717-4562-b3fc-2c963f66afa6");
            result = _eventValidatorRepository.IsSpaceIdExist(_eventInstance, findImageGuid);
            Assert.That(result, Is.False);
        }
    }
}