using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Infrastructure.Repositories;

namespace EventPlannerApp.Presentation;

public partial class AddEventPage : ContentPage
{
	private int? ImageIndex = 0;
    private readonly IServiceProvider _serviceProvider;
    public AddEventPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }
    private async void OnChooseImageClicked(object sender, EventArgs e)
    {
        var eventImagePage = _serviceProvider.GetRequiredService<EventImagePage>();
        eventImagePage.OnDataReturned += (data) =>
        {
            ImageIndex = data;
        };
        await Navigation.PushAsync(eventImagePage);
    }
    private async void OnAddEventClicked(object sender, EventArgs e)
    {
        var eventService = _serviceProvider.GetRequiredService<IEventService>();
        int eventId = await GenerateEventId();
        int userId = MainPage.UserId;
        var newEvent = new Event
        {
            Id = eventId,
            OrganizerId = userId,
            Name = EventNameEntry.Text,
            Description = EventDescriptionEntry.Text ?? "",
            Date = new DateTime(EventDatePicker.Date.Year,
                            EventDatePicker.Date.Month,
                            EventDatePicker.Date.Day,
                            EventTimePicker.Time.Hours,
                            EventTimePicker.Time.Minutes,
                            0),
            EndTime = new DateTime(EventDatePicker.Date.Year,
                            EventDatePicker.Date.Month,
                            EventDatePicker.Date.Day,
                            EventEndTimePicker.Time.Hours,
                            EventEndTimePicker.Time.Minutes,
                            0),
            ImageIndex = ImageIndex ?? 0,
            IsPublic = IsPublic.IsChecked,
            Location = EventLocationEntry.Text ?? "",
            MaxParticipants = int.Parse(MaxParticipantsEntry.Text ?? "10"),
            ParticipantsIds = new List<int>(){ userId },
            QRCode = $"event_{eventId}",
            Feedbacks = new Dictionary<string, Feedback>(),
            ViewCount = 0,
        };
        if (newEvent.Date < DateTime.Now)
        {
            await DisplayAlert("Error", "Event date cannot be in the past", "OK");
            return;
        }
        if (newEvent.MaxParticipants < 1)
        {
            await DisplayAlert("Error", "Event must have at least 1 participant", "OK");
            return;
        }
        if (newEvent.Name.Length < 3)
        {
            await DisplayAlert("Error", "Event name must be at least 3 characters long", "OK");
            return;
        }
        if (TagsEntry.Text is not null)
        {
            newEvent.Tags = TagsEntry.Text.Split(',', ' ').Select(tag => tag.Trim()).ToList();
        }
        if (TagsEntry.Text is null)
        {
            await DisplayAlert("Error", "Event must have at least 1 tag", "OK");
            return;
        }
        if (newEvent.EndTime < newEvent.Date)
        {
            await DisplayAlert("Error", "Event 'Endzeit' kann nicht richtig sein! ", "OK");
            return;
        }
        if (newEvent.OrganizerId is null)
        {
            await DisplayAlert("Error", "Organizer-ID fehlt!", "OK");
            return;
        }
        if (newEvent.ParticipantsIds is null)
        {
            await DisplayAlert("Error", "Participants-ID fehlt!", "OK");
            return;
        }
        await eventService.CreateEvent(newEvent);
        await Navigation.PopAsync();
    }
    Task<int> GenerateEventId()
    {
        var userService = _serviceProvider.GetRequiredService<IEventService>();
        var events = userService.Events;
        Random random = new Random();
        int eventId = random.Next(1000, 9999);
        if(events is not null)
        {
            while (events.Any(u => u.Id == eventId))
            {
                eventId = random.Next(1000, 9999);
            }
        }
        return Task.FromResult(eventId);
    }
}