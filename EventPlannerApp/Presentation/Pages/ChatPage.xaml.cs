using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using System.Collections.ObjectModel;

namespace EventPlannerApp.Presentation;

public partial class ChatPage : ContentPage
{
	private readonly IServiceProvider _serviceProvider;
    public ObservableCollection<Message>? Messages { get; set; } = [];
    public required Chat Chat { get; set; }
    public required String PartnerName { get; set; }
    public ChatPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    public void InitializeAsync(Chat chat)
    {
        Chat = chat;
        var userService = _serviceProvider.GetRequiredService<IUserService>();
        // Get the partner of the chat Where the partner is not the current user
        var partner = userService.Users.FirstOrDefault(u => chat.ParticipantIds.Contains(u.Id) && u.Id != MainPage.UserId);
        if (partner != null)
        {
            PartnerName = partner.Name;
        }
        LoadData();
        BindingContext = this;
    }

    private void LoadData()
    {
        var chatService = _serviceProvider.GetRequiredService<IChatService>();
        Messages = new ObservableCollection<Message>(chatService.GetMessages(Chat.Id));
    }

    private void OnMessageEntered(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(MessageEntry.Text))
                return;

            var chatService = _serviceProvider.GetRequiredService<IChatService>();

            var message = new Message
            {
                Id = chatService.GetMessages(Chat.Id).Count + 1,
                ChatId = Chat.Id,
                SenderId = MainPage.UserId,
                Content = MessageEntry.Text,
                SentAt = DateTime.Now
            };
            chatService.AddMessageToChat(Chat.Id, message);
            if (Messages != null)
            {
                Messages.Add(message);
            }
            else
            {
                Messages = [message];
            }

            MessageEntry.Text = "";

        }catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}