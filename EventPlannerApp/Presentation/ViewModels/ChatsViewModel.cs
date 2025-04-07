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
    public partial class ChatsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Chat>? userChats = new();
        [ObservableProperty]
        private ObservableCollection<User>? users = new();
        [ObservableProperty]
        private ObservableCollection<User>? filteredUsers = new();

        [ObservableProperty]
        private bool isUserSearchResultsVisible = false;
        [ObservableProperty]
        private string? userSearchText = string.Empty;

        public IRelayCommand<string?> SearchUserCommand => new RelayCommand<string?>(OnSearchUser);
        public IRelayCommand<User?> UserSelectedCommand => new RelayCommand<User?>(OnUserSelected);
        public IRelayCommand<Chat?> ChatSelectedCommand => new RelayCommand<Chat?>(OnChatSelected);

        private readonly IServiceProvider _serviceProvider;
        public ChatsViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadData();
        }

        private void LoadData()
        {
            var chatService = _serviceProvider.GetRequiredService<IChatService>();
            UserChats = chatService.UserChats;
            chatService.UserChats.CollectionChanged += (sender, e) =>
            {
                UserChats = chatService.UserChats;
            };
            var userService = _serviceProvider.GetRequiredService<IUserService>();
            Users = userService.Users;
            userService.Users.CollectionChanged += (sender, e) =>
            {
                Users = userService.Users;
            };
            FilteredUsers = new ObservableCollection<User>(Users);
        }

        private void OnSearchUser(string? searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                IsUserSearchResultsVisible = false;
                return;
            }
            FilteredUsers?.Clear();
            var results = Users?.Where(u => u.Name.ToLower().Contains(searchText.ToLower())).ToList();
            foreach (var user in results)
            {
                FilteredUsers.Add(user);
            }
            IsUserSearchResultsVisible = true;
        }

        private async void OnUserSelected(User? selectedUser)
        {
            if (selectedUser == null)
            {
                Console.WriteLine("Selected User is NULL");
                return;
            }
            IsUserSearchResultsVisible = false;
            UserSearchText = string.Empty;
            await StartChat(selectedUser);
        }

        private async Task StartChat(User selectedUser)
        {
            try
            {
                var chatPage = _serviceProvider.GetRequiredService<ChatPage>();
                // !!!
                if (UserChats.Any(c => c.ParticipantIds.Contains(selectedUser.Id)))
                {
                    var chatViewModel = _serviceProvider.GetRequiredService<ChatViewModel>();
                    chatViewModel.InitializeAsync(chat: UserChats.FirstOrDefault(c => c.ParticipantIds.Contains(selectedUser.Id)));
                    chatPage.BindingContext = chatViewModel;
                    await App.Current.MainPage.Navigation.PushAsync(chatPage);
                }
                else
                {
                    var chatService = _serviceProvider.GetRequiredService<IChatService>();
                    Chat chat = new()
                    {
                        Id = await chatService.GenerateChatIdAsync(),
                        ParticipantIds = new List<int> { MainViewModel.UserId, selectedUser.Id },
                        Messages = new List<Message>() { }
                    };
                    await chatService.CreateChat(chat);

                    var chatViewModel = _serviceProvider.GetRequiredService<ChatViewModel>();
                    chatViewModel.InitializeAsync(chat);
                    chatPage.BindingContext = chatViewModel;
                    await App.Current.MainPage.Navigation.PushAsync(chatPage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim starten von Chat: {ex.Message}");
            }
            finally
            {
                UserSearchText = string.Empty;
                IsUserSearchResultsVisible = false;
            }
        }

        private async void OnChatSelected(Chat? selectedChat)
        {
            if (selectedChat == null)
            {
                Console.WriteLine("Selected Chat is NULL");
                return;
            }
            try
            {
                var chatViewModel = _serviceProvider.GetRequiredService<ChatViewModel>();
                chatViewModel.InitializeAsync(selectedChat);
                var chatPage = _serviceProvider.GetRequiredService<ChatPage>();
                chatPage.BindingContext = chatViewModel;
                await App.Current.MainPage.Navigation.PushAsync(chatPage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim starten von Chat: {ex.Message}");
            }
        }
    }
}
