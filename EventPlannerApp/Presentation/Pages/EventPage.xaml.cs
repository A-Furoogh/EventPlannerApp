using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;

namespace EventPlannerApp.Presentation;

public partial class EventPage : ContentPage
{
	private readonly IServiceProvider _serviceProvider;
    private Event _event;
    private int _rating;
    public EventPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    public void InitializeAsync(Event _event)
    {
        this._event = _event;
        if (this._event.ParticipantsIds.Contains(MainPage.UserId))
        {
            Join_Button.IsEnabled = false;
            Join_Button.Text = "Shon Beigetreten";
        }
        if (this._event.OrganizerId == MainPage.UserId)
        {
            Edit_Button.IsEnabled = true;
            Edit_Button.IsVisible = true;
        }
        if(this._event.Feedbacks != null)
        {
            if(this._event.Feedbacks.Any(f => f.Value.UserId == MainPage.UserId))
            {
                FeedbackSection.IsVisible = false;
            }
        }
        IncrementViewCount();

        BindingContext = this._event;
    }

    private async void OnJoinEventButton_Clicked(object sender, EventArgs e)
    {
        if (this._event.ParticipantsIds.Contains(MainPage.UserId))
        {
            await DisplayAlert("Error", "You have already joined this event!", "OK");
            return;
        }
        var eventService = _serviceProvider.GetRequiredService<IEventService>();
        await eventService.AddUserToEvent(this._event.Id, MainPage.UserId);
        await DisplayAlert("Success", "You have joined the event!", "OK");
        Join_Button.IsEnabled = false;
    }

    private void OnEditEventButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var modifyEventPage = _serviceProvider.GetRequiredService<ModifyEventPage>();
            modifyEventPage.InitializeAsync(this._event);
            Navigation.PushAsync(modifyEventPage);

            Navigation.RemovePage(this);
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnStarClicked(object sender, EventArgs e)
    {
        if(sender is ImageButton starButton && starButton.CommandParameter is String parameter)
        {
            int selectedRating = int.Parse(parameter);

            _rating = selectedRating;

            for(int i = 1; i <= 5; i++)
            {
                var star = RatingStars.Children[i - 1] as ImageButton;
                if (i <= selectedRating)
                {
                    star.Source = "star_filled.png";
                }
                else
                {
                    star.Source = "star_empty.png";
                }
            }
        }
    }

    private async void OnSubmitFeedbackButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (_rating == 0)
            {
                await DisplayAlert("Error", "Bitte zuerst Bewertung!", "OK");
                return;
            }
            var eventService = _serviceProvider.GetRequiredService<IEventService>();
            Feedback feedback = new Feedback
            {
                UserId = MainPage.UserId,
                EventId = this._event.Id,
                Rating = _rating,
                Comment = CommentEntry.Text ?? ""
            };
            await eventService.AddFeedback(feedback);

            FeedbackSection.IsVisible = false;

            InitializeAsync(this._event);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void IncrementViewCount()
    {
        var eventService = _serviceProvider.GetRequiredService<IEventService>();
        await eventService.IncrementViewCount(this._event.Id);
    }
}