using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using System.Collections.ObjectModel;

namespace EventPlannerApp.Presentation;

public partial class ChatsPage : ContentPage
{
	private readonly IServiceProvider _serviceProvider;
    public ObservableCollection<Chat>? Chats { get; set; }
    public ObservableCollection<User>? Users { get; set; }
    public ObservableCollection<User>? FilteredUsers { get; set; }
public ChatsPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _serviceProvider = serviceProvider;
        LoadData();
        BindingContext = this;
    }
    private void LoadData()
    {
        var chatService = _serviceProvider.GetRequiredService<IChatService>();
        //chatService.GetAllChats();
        Chats = chatService.UserChats;
        chatService.UserChats.CollectionChanged += (sender, e) =>
        {
            Chats = chatService.UserChats;
        };

        var userService = _serviceProvider.GetRequiredService<IUserService>();
        Users = userService.Users;
        userService.Users.CollectionChanged += (sender, e) =>
        {
            Users = userService.Users;
        };
        FilteredUsers = new ObservableCollection<User>(Users);
    }
    private void OnUserSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue.ToLower();
        FilteredUsers?.Clear();

        if (String.IsNullOrWhiteSpace(searchText))
        {
            UserSearchResults.IsVisible = false;
            return;
        }

        var results = Users.Where(u => u.Name.ToLower().Contains(searchText)).ToList();

        foreach (var user in results)
        {
            FilteredUsers.Add(user);
        }

        UserSearchResults.IsVisible = true;
    }

    private void OnUserSelected(object sender, TappedEventArgs e)
    {
        if(e.Parameter is User selectedUser)
        {
            UserSearchBar.Text = selectedUser.Name;
            UserSearchResults.IsVisible = false;
            UserSearchBar.Text = "";
            StartChat(selectedUser);
        }
    }

    private async void StartChat(User user)
    {
        var chatPage = _serviceProvider.GetRequiredService<ChatPage>();
        // !!!
        if (Chats.Any(c => c.ParticipantIds.Contains(user.Id)))
        {
            chatPage.InitializeAsync(chat: Chats.FirstOrDefault(c => c.ParticipantIds.Contains(user.Id)));
            await Navigation.PushAsync(chatPage);
        }
        else
        {
            var chatService = _serviceProvider.GetRequiredService<IChatService>();
            Chat chat = new()
            {
                Id = await chatService.GenerateChatIdAsync(),
                ParticipantIds = new List<int> { MainPage.UserId, user.Id },
                Messages = new List<Message>() { }
            };
            await chatService.CreateChat(chat);

            chatPage.InitializeAsync(chat);
            await Navigation.PushAsync(chatPage);
        }
    }

    private async void OnChatSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Chat selectedChat)
        {
            var chatPage = _serviceProvider.GetRequiredService<ChatPage>();
            chatPage.InitializeAsync(selectedChat);
            await Navigation.PushAsync(chatPage);
        }
    }
}