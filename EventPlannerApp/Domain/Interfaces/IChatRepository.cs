using EventPlannerApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Application.Interfaces
{
    public interface IChatRepository
    {
        Task<IEnumerable<Chat>> GetAllChats();
        Task<Chat> GetChatById(int id);
        Task CreateChat(Chat newChat);
        Task UpdateChat(Chat updatedChat);
        Task DeleteChat(int id);
        Task<IEnumerable<Chat>> GetAllChatsByUserId(int userId);
        Task AddParticipantsToChat(int chatId, List<int> userIds);
        Task AddMessageToChat(int chatId, Message message);
        ObservableCollection<Message> GetMessages(int chatId);
        ObservableCollection<Chat> Chats { get; }
        ObservableCollection<Chat> UserChats { get; }
        Task<int> GenerateChatIdAsync();
    }
}
