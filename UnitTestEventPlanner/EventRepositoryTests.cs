using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Infrastructure.Repositories;
using FakeItEasy;
using Firebase.Database;
using Firebase.Database.Query;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestEventPlanner
{
    public class EventRepositoryTests
    {
        private readonly IEventRepository _fakeEventRepository;

        public EventRepositoryTests()
        {
            _fakeEventRepository = A.Fake<IEventRepository>();
        }

        [Fact]
        public async Task GetEventById_ShouldReturnEvent_WhenEventExists()
        {
            // Arrange
            var eventId = 1;
            var expectedEvent = new Event { Id = eventId, Name = "Test Event" };

            // Set up the fake behavior
            A.CallTo(() => _fakeEventRepository.GetEventById(eventId))
                .Returns(expectedEvent);

            // Act
            var result = await _fakeEventRepository.GetEventById(eventId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(eventId);
            result.Name.Should().Be("Test Event");

            // Verify the method was called
            A.CallTo(() => _fakeEventRepository.GetEventById(eventId))
                .MustHaveHappenedOnceExactly();
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
            var result = await _fakeEventRepository.GetAllEvents();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedEvents);

            // Verify the method was called
            A.CallTo(() => _fakeEventRepository.GetAllEvents())
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CreateEvent_ShouldCreateEvent()
        {
            // Arrange
            var newEvent = new Event { Id = 1, Name = "New Event" };
            // Set up the fake behavior
            A.CallTo(() => _fakeEventRepository.CreateEvent(newEvent))
                .DoesNothing();
            // Act
            await _fakeEventRepository.CreateEvent(newEvent);
            // Verify the method was called
            A.CallTo(() => _fakeEventRepository.CreateEvent(newEvent))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateEvent_ShouldUpdateEvent()
        {
            // Arrange
            var updatedEvent = new Event { Id = 1, Name = "Updated Event" };
            // Set up the fake behavior
            A.CallTo(() => _fakeEventRepository.UpdateEvent(updatedEvent))
                .DoesNothing();
            // Act
            await _fakeEventRepository.UpdateEvent(updatedEvent);
            // Verify the method was called
            A.CallTo(() => _fakeEventRepository.UpdateEvent(updatedEvent))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteEvent_ShouldDeleteEvent()
        {
            // Arrange
            var eventId = 1;
            // Set up the fake behavior
            A.CallTo(() => _fakeEventRepository.DeleteEvent(eventId))
                .DoesNothing();
            // Act
            await _fakeEventRepository.DeleteEvent(eventId);
            // Verify the method was called
            A.CallTo(() => _fakeEventRepository.DeleteEvent(eventId))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetAllEventsByUserId_ShouldReturnEventsByUserId()
        {
            // Arrange
            var userId = 1;
            var expectedEvents = new List<Event>
            {
                new Event { Id = 1, Name = "Event 1", OrganizerId = userId },
                new Event { Id = 2, Name = "Event 2", OrganizerId = userId }
            };
            // Set up the fake behavior
            A.CallTo(() => _fakeEventRepository.GetAllEventsByUserId(userId))
                .Returns(expectedEvents);
            // Act
            var result = await _fakeEventRepository.GetAllEventsByUserId(userId);
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedEvents);
            // Verify the method was called
            A.CallTo(() => _fakeEventRepository.GetAllEventsByUserId(userId))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddUsersToEvent_ShouldAddUsersToEvent()
        {
            // Arrange
            var eventId = 1;
            var userIds = new List<int> { 1, 2 };
            // Set up the fake behavior
            A.CallTo(() => _fakeEventRepository.AddUsersToEvent(eventId, userIds))
                .DoesNothing();
            // Act
            await _fakeEventRepository.AddUsersToEvent(eventId, userIds);
            // Verify the method was called
            A.CallTo(() => _fakeEventRepository.AddUsersToEvent(eventId, userIds))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddFeedback_ShouldAddFeedback()
        {
            // Arrange
            var feedback = new Feedback { EventId = 1, UserId = 1, Comment = "Test Comment" };
            // Set up the fake behavior
            A.CallTo(() => _fakeEventRepository.AddFeedback(feedback))
                .DoesNothing();
            // Act
            await _fakeEventRepository.AddFeedback(feedback);
            // Verify the method was called
            A.CallTo(() => _fakeEventRepository.AddFeedback(feedback))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddUserToEvent_ShouldAddUserToEvent()
        {
            // Arrange
            var eventId = 1;
            var userId = 1;
            // Set up the fake behavior
            A.CallTo(() => _fakeEventRepository.AddUserToEvent(eventId, userId))
                .DoesNothing();
            // Act
            await _fakeEventRepository.AddUserToEvent(eventId, userId);
            // Verify the method was called
            A.CallTo(() => _fakeEventRepository.AddUserToEvent(eventId, userId))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task IncrementViewCount_ShouldIncrementViewCount()
        {
            // Arrange
            var eventId = 1;
            // Set up the fake behavior
            A.CallTo(() => _fakeEventRepository.IncrementViewCount(eventId))
                .DoesNothing();
            // Act
            await _fakeEventRepository.IncrementViewCount(eventId);
            // Verify the method was called
            A.CallTo(() => _fakeEventRepository.IncrementViewCount(eventId))
                .MustHaveHappenedOnceExactly();
        }
    }
}
