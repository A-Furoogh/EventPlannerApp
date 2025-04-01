using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Presentation.ViewModels
{
    public partial class ChatViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Message>? _messages;
        [ObservableProperty]
        private Chat? chat;
        [ObservableProperty]
        private string? partnerName;
        [ObservableProperty]
        private string? messageText;

        public IRelayCommand SendMessageCommand => new RelayCommand(SendMessage);

        private readonly IServiceProvider _serviceProvider;
        public ChatViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void InitializeAsync(Chat chat)
        {
            Chat = chat;
            var userService = _serviceProvider.GetRequiredService<IUserService>();
            var partner = userService.Users.FirstOrDefault(u => chat.ParticipantIds.Contains(u.Id) && u.Id != MainPage.UserId);
            if (partner != null)
            {
                PartnerName = partner.Name;
            }
            LoadMessage();
        }

        private void LoadMessage()
        {
            var chatService = _serviceProvider.GetRequiredService<IChatService>();
            Messages = new ObservableCollection<Message>(chatService.GetMessages(Chat.Id));
        }

        private void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(MessageText))
                return;
            try
            {
                var chatService = _serviceProvider.GetRequiredService<IChatService>();
                var message = new Message
                {
                    Id = chatService.GetMessages(Chat.Id).Count + 1,
                    ChatId = Chat.Id,
                    SenderId = MainPage.UserId,
                    Content = MessageText,
                    SentAt = DateTime.Now
                };
                chatService.AddMessageToChat(Chat.Id, message);
                if (Messages != null)
                {
                    Messages.Add(message);
                }
                else
                {
                    Messages = new ObservableCollection<Message> { message };
                }

                MessageText = string.Empty;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
