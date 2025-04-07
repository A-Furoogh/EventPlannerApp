using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Application.Services;
using EventPlannerApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Presentation.ViewModels
{
    public partial class AddEventViewModel : ObservableObject
    {
        private int? ImageIndex = 0;
        [ObservableProperty]
        private string? eventName;
        [ObservableProperty]
        private string? eventDescription;
        [ObservableProperty]
        private DateTime eventDate = DateTime.Now;
        [ObservableProperty]
        private TimeSpan eventTime = DateTime.Now.TimeOfDay;
        [ObservableProperty]
        private TimeSpan eventEndTime = DateTime.Now.TimeOfDay;
        [ObservableProperty]
        private string? eventLocation;
        [ObservableProperty]
        private string? maxParticipants;
        [ObservableProperty]
        private bool isPublic = true;
        [ObservableProperty]
        private string? tags;


        public IRelayCommand ChooseImageCommand => new RelayCommand(OnChooseImage);
        public IRelayCommand AddEventCommand => new RelayCommand(AddEvent);

        private readonly IServiceProvider _serviceProvider;
        public AddEventViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        private async void OnChooseImage()
        {
            var eventImageViewModel = _serviceProvider.GetRequiredService<EventImageViewModel>();
            var eventImagePage = _serviceProvider.GetRequiredService<EventImagePage>();
            eventImagePage.BindingContext = eventImageViewModel;
            eventImageViewModel.OnDataReturned += (sender, data) =>
            {
                ImageIndex = data;
            };
            var navigationService = _serviceProvider.GetRequiredService<INavigation>();
            await navigationService.PushAsync(eventImagePage);
        }

        private async void AddEvent()
        {
            var eventService = _serviceProvider.GetRequiredService<IEventService>();
            int eventId = await GenerateEventId();
            int userId = MainViewModel.UserId;
            var newEvent = new Event
            {
                Id = eventId,
                OrganizerId = userId,
                Name = EventName ?? "",
                Description = EventDescription ?? "",
                Date = new DateTime(EventDate.Year,
                                EventDate.Month,
                                EventDate.Day,
                                EventTime.Hours,
                                EventTime.Minutes,
                                0),
                EndTime = new DateTime(EventDate.Year,
                                EventDate.Month,
                                EventDate.Day,
                                EventEndTime.Hours,
                                EventEndTime.Minutes,
                                0),
                ImageIndex = ImageIndex ?? 0,
                IsPublic = IsPublic,
                Location = EventLocation ?? "",
                MaxParticipants = int.Parse(MaxParticipants ?? "10"),
                ParticipantsIds = new List<int>() { userId },
                QRCode = $"event_{eventId}",
                Feedbacks = new Dictionary<string, Feedback>(),
                ViewCount = 0,
            };
            if (newEvent.Name.Length < 3 || newEvent.Name is null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Event-Name muss mindestens 3 Zeichen lang sein", "OK");
                return;
            }
            if (newEvent.Date < DateTime.Now)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Event-Datum kann nicht stimmen!", "OK");
                return;
            }
            if (Tags is not null)
            {
                newEvent.Tags = Tags.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(tag => tag.Trim())
                                    .ToList();
            }
            if (Tags is null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Event muss mindestens 1 Tag haben", "OK");
                return;
            }
            if (newEvent.EndTime < newEvent.Date)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Event-Datum kann nicht stimmen!", "OK");
                return;
            }
            if (newEvent.OrganizerId is null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Event-Datum kann nicht stimmen!", "OK");
                return;
            }
            if (newEvent.ParticipantsIds is null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Event-Datum kann nicht stimmen!", "OK");
                return;
            }

            await eventService.CreateEvent(newEvent);

            var notificationService = _serviceProvider.GetRequiredService<NotificationService>();
            var notifyTime = newEvent.Date.AddHours(-1);
            notificationService.ScheduleNotification("Event Erinnerung"
                                                    , $"Ihr Event {newEvent.Name} startet in einer Stunde"
                                                    , notifyTime);

            var navigationService = _serviceProvider.GetRequiredService<INavigation>();
            await navigationService.PopAsync();
        }

        Task<int> GenerateEventId()
        {
            var userService = _serviceProvider.GetRequiredService<IEventService>();
            var events = userService.Events;
            Random random = new Random();
            int eventId = random.Next(1000, 9999);
            if (events is not null)
            {
                while (events.Any(u => u.Id == eventId))
                {
                    eventId = random.Next(1000, 9999);
                }
            }
            return Task.FromResult(eventId);
        }
    }
}
