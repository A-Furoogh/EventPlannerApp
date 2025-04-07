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
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<User>? users = new();
        [ObservableProperty]
        private ObservableCollection<Event>? filteredEvents = new();
        [ObservableProperty]
        private string? eventSearchText = string.Empty;

        public static int UserId { get; set; }
        public static Dictionary<int, string> UserNames = new();

        public IRelayCommand<Event?> EventSelectedCommand => new RelayCommand<Event?>(OnEventSelected);
        public IRelayCommand AddEventCommand => new RelayCommand(OnAddEvent);
        public IRelayCommand<string?> SearchEventCommand => new RelayCommand<string?>(OnSearchEvent);
        public IRelayCommand QrSearchCommand => new RelayCommand(OnQrSearch);
        public IRelayCommand LogoutCommand => new RelayCommand(OnLogout);

        private readonly IServiceProvider _serviceProvider;
        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadData();
        }

        private void LoadData()
        {
            var AuthenticationService = _serviceProvider.GetRequiredService<IAuthenticationService>();
            UserId = AuthenticationService.GetUserIdAsync();

            var eventService = _serviceProvider.GetRequiredService<IEventService>();
            FilteredEvents?.Clear();
            foreach (var ev in eventService.PublicEvents)
            {
                if (FilteredEvents is not null)
                {
                    FilteredEvents.Add(ev);
                }
            }

            eventService.PublicEvents.CollectionChanged += (sender, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (Event ev in e.NewItems)
                    {
                        if (FilteredEvents is not null)
                        {
                            if (!FilteredEvents.Contains(ev))
                            {
                                FilteredEvents.Add(ev);
                            }
                        }
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (Event ev in e.OldItems)
                    {
                        if (FilteredEvents is not null)
                        {
                            if (FilteredEvents.Contains(ev))
                            {
                                FilteredEvents.Remove(ev);
                            }
                        }
                    }
                }
            };

            _ = LoadUserNamesAsync();
        }

        public Task LoadUserNamesAsync()
        {
            var userService = _serviceProvider.GetRequiredService<IUserService>();
            foreach (var user in userService.Users)
            {
                UserNames[user.Id] = user.Name;
            }
            return Task.CompletedTask;
        }

        public static async Task<string> GetUserNameAsync(int userId)
        {
            if (UserNames.TryGetValue(userId, out string? value))
            {
                return await Task.FromResult(value);
            }
            return string.Empty;
        }

        private async void OnEventSelected(Event? selectedEvent)
        {
            if (selectedEvent != null)
            {
                try
                {
                    var eventViewModel = _serviceProvider.GetRequiredService<EventViewModel>();
                    eventViewModel.InitializeAsync(selectedEvent);
                    var eventPage = _serviceProvider.GetRequiredService<EventPage>();
                    eventPage.BindingContext = eventViewModel;
                    await App.Current.MainPage.Navigation.PushAsync(eventPage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private async void OnAddEvent()
        {
            var addEventViewModel = _serviceProvider.GetRequiredService<AddEventViewModel>();
            var addEventPage = _serviceProvider.GetRequiredService<AddEventPage>();
            addEventPage.BindingContext = addEventViewModel;
            await App.Current.MainPage.Navigation.PushAsync(addEventPage);
        }

        private async void OnQrSearch()
        {
            try
            {
                var qrSearchViewModel = _serviceProvider.GetRequiredService<QrScanViewModel>();
                var qrSearchPage = _serviceProvider.GetRequiredService<QrScanPage>();
                qrSearchPage.BindingContext = qrSearchViewModel;
                await App.Current.MainPage.Navigation.PushAsync(qrSearchPage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async void OnLogout()
        {
            try
            {
                var answer = await App.Current.MainPage.DisplayAlert("Ausloggen", "Möchten Sie ausloggen?", "Ja", "Nein");
                if (answer)
                {
                    var AuthenticationService = _serviceProvider.GetRequiredService<IAuthenticationService>();
                    AuthenticationService.Logout();
                    Preferences.Remove("Username");
                    Preferences.Remove("Password");
                    var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
                    var loginPage = _serviceProvider.GetRequiredService<LoginPage>();
                    loginPage.BindingContext = loginViewModel;
                    App.Current.MainPage = new NavigationPage(loginPage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void OnSearchEvent(string? searchText)
        {
            var eventService = _serviceProvider.GetRequiredService<IEventService>();
            FilteredEvents?.Clear();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                foreach (var ev in eventService.PublicEvents)
                {
                    if (FilteredEvents is not null)
                    {
                        FilteredEvents.Add(ev);
                    }
                }
                return;
            }
            var results = eventService.PublicEvents.Where(e => e.Name.ToLower()
                          .Contains(searchText) || e.Tags.Any(t => t.ToLower()
                          .Contains(searchText))).ToList();

            foreach (var ev in results)
            {
                if (FilteredEvents is not null)
                {
                    FilteredEvents.Add(ev);
                }
            }
            EventSearchText = string.Empty;
        }
    }
}
