using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Application.Services;
using EventPlannerApp.Domain.Entities;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestEventPlanner
{
    public class EventServiceTests
    {
        private readonly IEventRepository _fakeEventRepository;
        private readonly IEventService _fakeEventService;

        public EventServiceTests()
        {
            _fakeEventRepository = A.Fake<IEventRepository>();
            _fakeEventService = new EventService(_fakeEventRepository);
        }

        [Fact]
        public async Task GetEventById_ShouldReturnEvent_WhenEventExists()
        {
            // Arrange
            var eventId = 1;
            var expectedEvent = new Event { Id = eventId, Name = "TestEvent" };
            // Set up the fake behavior
            A.CallTo(() => _fakeEventRepository.GetEventById(eventId))
                .Returns(expectedEvent);
            // Act
            var result = await _fakeEventService.GetEventById(eventId);
            // Assert
            result.Should().Be(expectedEvent);
        }

        [Fact]
        public async Task GetEventById_ShouldThrowArgumentException_WhenEventIdIsZero()
        {
            // Arrange
            var eventId = 0;
            // Act
            Func<Task> act = async () => await _fakeEventService.GetEventById(eventId);
            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("EventId muss größer als 0 sein");
        }

        [Fact]
        public async Task GetEventById_ShouldThrowArgumentException_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;
            // Set up the fake behavior
            A.CallTo(() => _fakeEventRepository.GetEventById(eventId))
                .Returns(Task.FromResult<Event>(null));
            // Act
            Func<Task> act = async () => await _fakeEventService.GetEventById(eventId);
            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Event mit dieser Id existiert nicht");
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnAllEvents()
        {
            // Arrange
            var expectedEvents = new List<Event>
            {
                new Event { Id = 1, Name = "Event 1" },
                new Event { Id = 2, Name = "Event 2" }
            };
            // Set up the fake behavior
            A.CallTo(() => _fakeEventRepository.GetAllEvents())
                .Returns(expectedEvents);
            // Act
            var result = await _fakeEventService.GetAllEvents();
            // Assert
            result.Should().BeEquivalentTo(expectedEvents);
        }


        [Fact]
        public async Task CreateEvent_ShouldThrowArgumentException_WhenEventIsNull()
        {
            // Arrange
            Event newEvent = null;
            // Act
            Func<Task> act = async () => await _fakeEventService.CreateEvent(newEvent);
            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Event kann nicht null sein");
        }
    }
}
