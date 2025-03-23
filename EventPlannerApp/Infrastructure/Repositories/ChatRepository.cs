using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Presentation;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly FirebaseClient _firebaseClient;
        public ObservableCollection<Chat> UserChats { get; private set; } = [];
        public ObservableCollection<Chat> Chats { get; private set; } = [];
        public ChatRepository(FirebaseClient firebaseClient)
        {
            _firebaseClient = firebaseClient;
            InitializeRealTimeUpdates();
            LoadData();
        }
        private void LoadData()
        {
            UserChats = new ObservableCollection<Chat>(_firebaseClient
                .Child("Chats")
                .OnceAsync<Chat>()
                .Result
                .Select(f => f.Object)
                .Where(c => c.ParticipantIds.Contains(MainPage.UserId)));
        }
        private void InitializeRealTimeUpdates()
        {
            _firebaseClient
                .Child("Chats")
                .AsObservable<Chat>()
                .Subscribe(update =>
                {
                    var chat = update.Object;
                    switch (update.EventType)
                    {
                        case Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate:
                            AddOrUpdateChat(chat);
                            break;
                        case Firebase.Database.Streaming.FirebaseEventType.Delete:
                            RemoveChat(chat.Id);
                            break;
                    }
                });
        }
        private void AddOrUpdateChat(Chat chat)
        {
            //var existingChat = Chats.FirstOrDefault(c => c.Id == chat.Id);
            //if (existingChat != null)
            //{
            //    // Update existing chat
            //    var index = Chats.IndexOf(existingChat);
            //    Chats[index] = chat;
            //}
            //else
            //{
            //    // Add new chat
            //    Chats.Add(chat);
            //}

            if (chat.ParticipantIds.Contains(MainPage.UserId))
            {
                var userChat = UserChats.FirstOrDefault(c => c.Id == chat.Id);
                if (userChat != null)
                {
                    // Update existing user chat
                    var index = UserChats.IndexOf(userChat);
                    UserChats[index] = chat;
                }
                else
                {
                    // Add new user chat
                    UserChats.Add(chat);
                }
            }
            else
            {
                var userChat = UserChats.FirstOrDefault(c => c.Id == chat.Id);
                if (userChat != null)
                {
                    // Remove user chat
                    UserChats.Remove(userChat);
                }
            }
        }
        private void RemoveChat(int chatId)
        {
            var chat = Chats.FirstOrDefault(c => c.Id == chatId);
            if (chat != null)
            {
                Chats.Remove(chat);
            }

            var userChat = UserChats.FirstOrDefault(c => c.Id == chatId);
            if (userChat != null)
            {
                UserChats.Remove(userChat);
            }
        }
        public  async Task<Chat> GetChatById(int chatId)
        {
            return await _firebaseClient
                .Child("Chats")
                .Child(chatId.ToString())
                .OnceSingleAsync<Chat>();
        }
        public async Task<IEnumerable<Chat>> GetAllChats()
        {
            var chats = await _firebaseClient
                .Child("Chats")
                .OnceAsync<Chat>();
            return chats.Select(f => f.Object);
        }
        public async Task CreateChat(Chat newChat)
        {
            await _firebaseClient
                .Child("Chats")
                .Child(newChat.Id.ToString())
                .PutAsync(newChat);
        }
        public async Task UpdateChat(Chat newChat)
        {
            await _firebaseClient
                .Child("Chats")
                .Child(newChat.Id.ToString())
                .PutAsync(newChat);
            AddOrUpdateChat(newChat);
        }
        public async Task DeleteChat(int chatId)
        {
            await _firebaseClient
                .Child("Chats")
                .Child(chatId.ToString())
                .DeleteAsync();
        }
        public async Task<IEnumerable<Chat>> GetAllChatsByUserId(int userId)
        {
            var chats = await _firebaseClient
                .Child("Chats")
                .OnceAsync<Chat>();
            return chats.Select(f => f.Object).Where(c => c.ParticipantIds.Contains(userId));
        }
        public async Task AddParticipantsToChat(int chatId, List<int> userIds)
        {
            var chat = await GetChatById(chatId);
            chat.ParticipantIds.AddRange(userIds);
            await UpdateChat(chat);
        }
        // !! !! !!
        public async Task AddMessageToChat(int chatId, Message message)
        {
            var chat = await GetChatById(chatId);
            if (chat.Messages == null)
            {
                chat.Messages = new List<Message>();
                chat.Messages.Add(message);
            }
            else
            {
                chat.Messages.Add(message);
            }
            await UpdateChat(chat);
        }
        public ObservableCollection<Message> GetMessages(int chatId)
        {
            var chat = UserChats.FirstOrDefault(c => c.Id == chatId);
            if (chat != null)
            {
                chat.Messages ??= new List<Message>();
                return new ObservableCollection<Message>(chat.Messages);
            }
            return [];
        }
        public async Task<int> GenerateChatIdAsync()
        {
            var chats = await GetAllChats();
            Random random = new();
            int chatId = random.Next(1000, 9999);
            while (chats.Any(c => c.Id == chatId))
            {
                chatId = random.Next(1000, 9999);
            }
            return chatId;
        }
    }
}
