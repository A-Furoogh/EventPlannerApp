using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Presentation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EventPlannerApp.Presentation
{
    public partial class MainPage : ContentPage
    {
        private readonly IServiceProvider _serviceProvider;
        public ObservableCollection<User>? Users { get; set; }
        public ObservableCollection<Event>? FilteredEvents { get; set; } = [];
        public static int UserId { get; set; }
        public static Dictionary<int, string> UserNames = [];

        public MainPage(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            LoadData();
            BindingContext = this;
        }
        private void LoadData()
        {
            var AuthenticationService = _serviceProvider.GetRequiredService<IAuthenticationService>();
            int userId = AuthenticationService.GetUserIdAsync();
            UserId = userId;

            var eventService = _serviceProvider.GetRequiredService<IEventService>();

            FilteredEvents?.Clear();
            foreach (var ev in eventService.PublicEvents)
            {
                FilteredEvents?.Add(ev);
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
                                FilteredEvents?.Remove(ev);
                            }
                        }
                    }
                }
            };

            _ = LoadUserNamesAsync();
        }
        private void OnEventSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Event selectedEvent)
            {
                var eventPage = _serviceProvider.GetRequiredService<EventPage>();
                eventPage.InitializeAsync(selectedEvent);
                Navigation.PushAsync(eventPage);
            }
        }
        private async void OnAddEvent_Clicked(object sender, EventArgs e)
        {
            var AddEventPage = _serviceProvider.GetRequiredService<AddEventPage>();
            await Navigation.PushAsync(AddEventPage);
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

        private void OnEventSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue.ToLower();
            FilteredEvents?.Clear();

            var eventService = _serviceProvider.GetRequiredService<IEventService>();

            if (string.IsNullOrEmpty(searchText))
            {
                foreach (var ev in eventService.PublicEvents)
                {
                    if(FilteredEvents is not null)
                    {
                        FilteredEvents.Add(ev);
                    }
                }
            }
            else
            {
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
            }
        }

        private void OnQrSearchButton_Clicked(object sender, EventArgs e)
        {
            var qrScanPage = _serviceProvider.GetRequiredService<QrScanPage>();
            Navigation.PushAsync(qrScanPage);
        }

        private async void OnLogOutButton_Clicked(object sender, EventArgs e)
        {
            // First ask the user if they are sure they want to log out
            var answer = await DisplayAlert("Ausloggen", "Sie möchten ausloggen?", "Ja", "Nein");
            if (answer)
            {
                var AuthenticationService = _serviceProvider.GetRequiredService<IAuthenticationService>();
                AuthenticationService.Logout();
                var loginPage = _serviceProvider.GetRequiredService<LoginPage>();
                App.Current.MainPage = new NavigationPage(loginPage);
            }
        }
    }

}
