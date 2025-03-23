using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;

namespace EventPlannerApp.Presentation;

public partial class ModifyEventPage : ContentPage
{
	private readonly IServiceProvider _serviceProvider;
    private Event _event;
    public ModifyEventPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    public void InitializeAsync(Event _event)
    {
        this._event = _event;
        BindingContext = this._event;
    }

    private async void OnSaveEventButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var eventService = _serviceProvider.GetRequiredService<IEventService>();
            await eventService.UpdateEvent(this._event);
            await DisplayAlert("Erfolgreich", "Event erfolgreich bearbeitet!", "OK");
        }catch (Exception ex)
        {
            await DisplayAlert("Fehler", $"Event konnte nicht bearbeitet werden!\n{ex}", "OK");
        }
    }

    private async void OnDeleteEventButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var eventService = _serviceProvider.GetRequiredService<IEventService>();
            await eventService.DeleteEvent(this._event.Id);
            await DisplayAlert("Erfolgreich", "Event erfolgreich gelöscht!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Fehler", $"Event konnte nicht gelöscht werden!\n{ex}", "OK");
        }
    }
}