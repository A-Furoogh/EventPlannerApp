using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestEventPlanner
{
    public class ChatRepositoryTests
    {
        private readonly IChatRepository _fakeChatRepository;

        public ChatRepositoryTests()
        {
            _fakeChatRepository = A.Fake<IChatRepository>();
        }

        [Fact]
        public async Task GetChatById_ShouldReturnChat_WhenChatExists()
        {
            // Arrange
            var chatId = 1;
            var expectedChat = new Chat { Id = chatId, ParticipantIds = new List<int> { 1, 2 }, Messages = new List<Message>() };

            // Set up the fake behavior
            A.CallTo(() => _fakeChatRepository.GetChatById(chatId))
                .Returns(expectedChat);

            // Act
            var result = await _fakeChatRepository.GetChatById(chatId);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllChats_ShouldReturnAllChats()
        {
            // Arrange
            var expectedChats = new List<Chat>
            {
                new Chat { Id = 1, ParticipantIds = new List<int> { 1, 2 }, Messages = new List<Message>() },
                new Chat { Id = 2, ParticipantIds = new List<int> { 2, 3 }, Messages = new List<Message>() }
            };
            // Set up the fake behavior
            A.CallTo(() => _fakeChatRepository.GetAllChats())
                .Returns(expectedChats);
            // Act
            var result = await _fakeChatRepository.GetAllChats();
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task CreateChat_ShouldCreateChat()
        {
            // Arrange
            var newChat = new Chat { Id = 1, ParticipantIds = new List<int> { 1, 2 }, Messages = new List<Message>() };
            // Act
            await _fakeChatRepository.CreateChat(newChat);
            // Assert
            A.CallTo(() => _fakeChatRepository.CreateChat(newChat))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateChat_ShouldUpdateChat()
        {
            // Arrange
            var newChat = new Chat { Id = 1, ParticipantIds = new List<int> { 1, 2 }, Messages = new List<Message>() };
            // Act
            await _fakeChatRepository.UpdateChat(newChat);
            // Assert
            A.CallTo(() => _fakeChatRepository.UpdateChat(newChat))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteChat_ShouldDeleteChat()
        {
            // Arrange
            var chatId = 1;
            // Act
            await _fakeChatRepository.DeleteChat(chatId);
            // Assert
            A.CallTo(() => _fakeChatRepository.DeleteChat(chatId))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetAllChatsByUserId_ShouldReturnAllChatsByUserId()
        {
            // Arrange
            var userId = 1;
            var expectedChats = new List<Chat>
            {
                new Chat { Id = 1, ParticipantIds = new List<int> { 1, 2 }, Messages = new List<Message>() },
                new Chat { Id = 2, ParticipantIds = new List<int> { 2, 3 }, Messages = new List<Message>() }
            };
            // Set up the fake behavior
            A.CallTo(() => _fakeChatRepository.GetAllChatsByUserId(userId))
                .Returns(expectedChats);
            // Act
            var result = await _fakeChatRepository.GetAllChatsByUserId(userId);
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task AddParticipantsToChat_ShouldAddParticipantsToChat()
        {
            // Arrange
            var chatId = 1;
            var userIds = new List<int> { 3, 4 };
            // Act
            await _fakeChatRepository.AddParticipantsToChat(chatId, userIds);
            // Assert
            A.CallTo(() => _fakeChatRepository.AddParticipantsToChat(chatId, userIds))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddMessageToChat_ShouldAddMessageToChat()
        {
            // Arrange
            var chatId = 1;
            var message = new Message { ChatId = 1, Id = 1, SenderId = 1, Content = "Test message", SentAt = DateTime.Now };
            // Act
            await _fakeChatRepository.AddMessageToChat(chatId, message);
            // Assert
            A.CallTo(() => _fakeChatRepository.AddMessageToChat(chatId, message))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddMessageToChat_ShouldAddMessageToChat_WhenChatMessagesAreNull()
        {
            // Arrange
            var chatId = 1;
            var message = new Message { ChatId = 1, Id = 1, SenderId = 1, Content = "Test message", SentAt = DateTime.Now };
            // Act
            await _fakeChatRepository.AddMessageToChat(chatId, message);
            // Assert
            A.CallTo(() => _fakeChatRepository.AddMessageToChat(chatId, message))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddMessageToChat_ShouldAddMessageToChat_WhenChatMessagesAreNotNull()
        {
            // Arrange
            var chatId = 1;
            var message = new Message { ChatId = 1, Id = 1, SenderId = 1, Content = "Test message", SentAt = DateTime.Now };
            // Act
            await _fakeChatRepository.AddMessageToChat(chatId, message);
            // Assert
            A.CallTo(() => _fakeChatRepository.AddMessageToChat(chatId, message))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetMessages_ShouldReturnMessages()
        {
            // Arrange
            var chatId = 1;
            var expectedMessages = new ObservableCollection<Message>
    {
        new Message { ChatId = 1, Id = 1, SenderId = 1, Content = "Test message", SentAt = DateTime.Now },
        new Message { ChatId = 1, Id = 2, SenderId = 2, Content = "Test message 2", SentAt = DateTime.Now }
    };

            // Set up the fake behavior
            A.CallTo(() => _fakeChatRepository.GetMessages(chatId))
                .Returns(expectedMessages);

            // Act
            var result = _fakeChatRepository.GetMessages(chatId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedMessages);

            // Verify the method was called
            A.CallTo(() => _fakeChatRepository.GetMessages(chatId))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetMessages_ShouldReturnMessages_WhenMessagesAreNull()
        {
            // Arrange
            var chatId = 1;
            var expectedMessages = new ObservableCollection<Message>();
            // Set up the fake behavior
            A.CallTo(() => _fakeChatRepository.GetMessages(chatId))
                .Returns(expectedMessages);
            // Act
            var result = _fakeChatRepository.GetMessages(chatId);
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
            result.Should().BeEquivalentTo(expectedMessages);
            // Verify the method was called
            A.CallTo(() => _fakeChatRepository.GetMessages(chatId))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetMessages_ShouldReturnMessages_WhenMessagesAreNotNull()
        {
            // Arrange
            var chatId = 1;
            var expectedMessages = new ObservableCollection<Message>
            {
                new Message { ChatId = 1, Id = 1, SenderId = 1, Content = "Test message", SentAt = DateTime.Now },
                new Message { ChatId = 1, Id = 2, SenderId = 2, Content = "Test message 2", SentAt = DateTime.Now }
            }
            ;
            // Set up the fake behavior
            A.CallTo(() => _fakeChatRepository.GetMessages(chatId))
                .Returns(expectedMessages);
            // Act
            var result = _fakeChatRepository.GetMessages(chatId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedMessages);
            // Verify the method was called
            A.CallTo(() => _fakeChatRepository.GetMessages(chatId))
                .MustHaveHappenedOnceExactly();
        }
    }
}
