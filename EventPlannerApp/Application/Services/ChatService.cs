using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<Chat> GetChatById(int chatId)
        {
            if (chatId <= 0)
            {
                throw new ArgumentException("Ungültige Chat ID");
            }
            return await _chatRepository.GetChatById(chatId);
        }

        public async Task<IEnumerable<Chat>> GetAllChats()
        {
            if (_chatRepository.Chats.Count == 0)
            {
                throw new KeyNotFoundException("Keine Chats gefunden");
            }
            return await _chatRepository.GetAllChats();
        }

        public async Task CreateChat(Chat newChat)
        {
            if (newChat == null)
            {
                throw new ArgumentNullException("Chat darf nicht null sein");
            }
            if (newChat.Id <= 0)
            {
                throw new ArgumentException("Ungültige Chat ID");
            }
            await _chatRepository.CreateChat(newChat);
        }

        public async Task UpdateChat(Chat newChat)
        {
            if (newChat == null)
            {
                throw new ArgumentNullException("Chat darf nicht null sein");
            }
            await _chatRepository.UpdateChat(newChat);
        }

        public async Task DeleteChat(int chatId)
        {
            if (chatId <= 0)
            {
                throw new ArgumentException("Ungültige Chat ID");
            }
            await _chatRepository.DeleteChat(chatId);
        }
        public async Task<IEnumerable<Chat>> GetAllChatsByUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Ungültige User ID");
            }
            if (_chatRepository.UserChats.All(c => c.ParticipantIds.All(p => p != userId)))
            {
                throw new KeyNotFoundException("Keine Chats gefunden");
            }
            return await _chatRepository.GetAllChatsByUserId(userId);
        }
        public async Task AddParticipantsToChat(int chatId, List<int> userIds)
        {
            if (chatId <= 0)
            {
                throw new ArgumentException("Ungültige Chat ID");
            }
            if (userIds == null)
            {
                throw new ArgumentNullException("User IDs dürfen nicht null sein");
            }
            if (userIds.Count == 0)
            {
                throw new ArgumentException("User IDs dürfen nicht leer sein");
            }
            await _chatRepository.AddParticipantsToChat(chatId, userIds);
        }
        public async Task AddMessageToChat(int chatId, Message message)
        {
            if (chatId <= 0)
            {
                throw new ArgumentException("Ungültige Chat ID");
            }
            if (message == null)
            {
                throw new ArgumentNullException("Message darf nicht null sein");
            }
            if (message.Id <= 0)
            {
                throw new ArgumentException("Ungültige Message ID");
            }
            await _chatRepository.AddMessageToChat(chatId, message);
        }
        public ObservableCollection<Message> GetMessages(int chatId)
        {
            if (chatId <= 0)
            {
                throw new ArgumentException("Ungültige Chat ID");
            }

            return _chatRepository.GetMessages(chatId);
        }
        public ObservableCollection<Chat> Chats => _chatRepository.Chats;
        public ObservableCollection<Chat> UserChats => _chatRepository.UserChats;
        public async Task<int> GenerateChatIdAsync()
        {
            return await _chatRepository.GenerateChatIdAsync();
        }
    }
}
