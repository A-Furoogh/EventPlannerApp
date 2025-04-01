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
    public partial class EventViewModel : ObservableObject
    {
        [ObservableProperty]
        private Event? myEvent;
        [ObservableProperty] private string? name;
        [ObservableProperty] private int? imageIndex;
        [ObservableProperty] private string? description;
        [ObservableProperty] private DateTime? date;
        [ObservableProperty] private DateTime? endTime;
        [ObservableProperty] private string? location;
        [ObservableProperty] private int? organizerId;
        [ObservableProperty] private ObservableCollection<Feedback>? feedbacks;
        [ObservableProperty] private int rating;
        [ObservableProperty] private string? comment;
        [ObservableProperty]
        private bool isFeedbackVisible = true;
        [ObservableProperty]
        private bool isJoinButtonEnabled = true;
        [ObservableProperty]
        private string joinButtonText = "Beitreten";
        [ObservableProperty]
        private bool isEditButtonVisible = false;
        [ObservableProperty]
        private List<string> starSources = new List<string>
        {
            "star_empty.png", "star_empty.png", "star_empty.png", "star_empty.png", "star_empty.png"
        };

        public IRelayCommand JoinCommand => new RelayCommand(Join);
        public IRelayCommand EditCommand => new RelayCommand(Edit);
        public IRelayCommand<string> StarClickedCommand => new RelayCommand<string>(StarClicked);
        public IRelayCommand SubmitFeedbackCommand => new RelayCommand(SubmitFeedback);

        private readonly IServiceProvider _serviceProvider;
        public EventViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void InitializeAsync(Event myEvent)
        {
            MyEvent = myEvent;
            Name = myEvent.Name;
            ImageIndex = myEvent.ImageIndex;
            Description = myEvent.Description;
            Date = myEvent.Date;
            EndTime = myEvent.EndTime;
            Location = myEvent.Location;
            OrganizerId = myEvent.OrganizerId;
            Feedbacks = new ObservableCollection<Feedback>(myEvent.Feedbacks?.Values.ToList() ?? new List<Feedback>());
            Rating = myEvent.Rating;

            if (myEvent.ParticipantsIds.Contains(MainPage.UserId))
            {
                IsJoinButtonEnabled = false;
                JoinButtonText = "Shon Beigetreten";
            }
            if (myEvent.OrganizerId == MainPage.UserId)
            {
                IsEditButtonVisible = true;
                IsEditButtonVisible = true;
            }
            if (myEvent.Feedbacks != null)
            {
                if (myEvent.Feedbacks.Any(f => f.Value.UserId == MainPage.UserId))
                {
                    IsFeedbackVisible = false;
                }
            }
            _ = IncrementViewCount();
        }

        private async Task IncrementViewCount()
        {
            var eventService = _serviceProvider.GetRequiredService<IEventService>();
            await eventService.IncrementViewCount(MyEvent.Id);
        }

        private async void Join()
        {
            if (MyEvent.ParticipantsIds.Contains(MainPage.UserId))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Sie haben bereits an diesem Event teilgenommen!", "OK");
                return;
            }
            try
            {
                var eventService = _serviceProvider.GetRequiredService<IEventService>();
                await eventService.AddUserToEvent(MyEvent.Id, MainPage.UserId);
                await App.Current.MainPage.DisplayAlert("Success", "Sie sind das Event beigetreten!", "OK");
                IsJoinButtonEnabled = false;
                JoinButtonText = "Shon Beigetreten";
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ein Fehler ist aufgetreten!" + e.Message, "OK");
            }
        }

        private void Edit()
        {
            try
            {
                var modifyEventPage = _serviceProvider.GetRequiredService<ModifyEventPage>();
                var modifyEventViewModel = _serviceProvider.GetRequiredService<ModifyEventViewModel>();
                modifyEventViewModel.MyEvent = MyEvent;
                modifyEventPage.BindingContext = modifyEventViewModel;
                var navigationService = _serviceProvider.GetRequiredService<INavigation>();
                
                navigationService.PushAsync(modifyEventPage);
                navigationService.PopAsync();
            }
            catch (Exception e)
            {
                App.Current.MainPage.DisplayAlert("Error", "Ein Fehler ist aufgetreten!:" + e.Message, "OK");
            }
        }

        private void StarClicked(string? ratingString)
        {
            if (ratingString is not null)
            {
                try
                {
                    Rating = int.Parse(ratingString);
                    Console.WriteLine($"Rating: {Rating}, StarSources.Count: {StarSources.Count}");
                    for (int i = 0; i <= 4; i++)
                    {
                        Console.WriteLine($"Loop i: {i}");
                        StarSources[i] = (i < Rating) ? "star_filled.png" : "star_empty.png";
                    }
                    OnPropertyChanged(nameof(StarSources));
                }
                catch (Exception e)
                {
                    App.Current.MainPage.DisplayAlert("Error", "Ein Fehler ist aufgetreten!" + e.Message, "OK");
                }
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Error", "Ein Fehler ist beim Command-Parameter aufgetreten!", "OK");
            }
        }

        private async void SubmitFeedback()
        {
            try
            {
                if (Rating == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Bitte bewerten Sie das Event!", "OK");
                    return;
                }
                var eventService = _serviceProvider.GetRequiredService<IEventService>();
                Feedback feedback = new Feedback
                {
                    UserId = MainPage.UserId,
                    EventId = MyEvent.Id,
                    Rating = Rating,
                    Comment = Comment,
                };
                await eventService.AddFeedback(feedback);

                IsFeedbackVisible = false;

                InitializeAsync(MyEvent);
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ein Fehler ist aufgetreten!:" + e.Message, "OK");
            }
        }
    }
}
