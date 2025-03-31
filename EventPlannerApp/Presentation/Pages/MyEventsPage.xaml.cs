using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Presentation.ViewModels;
using System.Collections.ObjectModel;

namespace EventPlannerApp.Presentation;

public partial class MyEventsPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public ObservableCollection<Event>? UserEvents { get; set; } = [];

    public MyEventsPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _serviceProvider = serviceProvider;
        LoadData();
        BindingContext = this;
    }

    private void LoadData()
    {
        var eventService = _serviceProvider.GetRequiredService<IEventService>();
        UserEvents = eventService.UserEvents;
        UserEvents.CollectionChanged += (sender, e) =>
        {
            UserEvents = eventService.UserEvents;
        };

        //SchedulerAppointments = ConvertToSchedulerAppointments(UserEvents);
        //SchedulerAppointments.CollectionChanged += (sender, e) =>
        //{
        //    SchedulerAppointments = ConvertToSchedulerAppointments(UserEvents);
        //};
    }

    private void OnEventSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Event selectedEvent)
        {
            //Navigation.PushAsync(new EventDetailsPage(selectedEvent));
        }
    }

    private void OnQrButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Image image && image.Parent is VerticalStackLayout grid && grid.BindingContext is Event selectedEvent)
        {
            var qrCodeViewModel = _serviceProvider.GetRequiredService<QrCodeViewModel>();
            qrCodeViewModel.EventId = selectedEvent.Id;
            qrCodeViewModel.GenerateQrCode();
            Navigation.PushAsync(new QrCodePage(qrCodeViewModel));
        }
    }
    
    private void OnAnalyticsButton_Clicked(object sender, EventArgs e)
    {
        var analyticsPage = _serviceProvider.GetRequiredService<AnalyticsPage>();
        Navigation.PushAsync(analyticsPage);
    }

    //private ObservableCollection<SchedulerAppointment> ConvertToSchedulerAppointments(ObservableCollection<Event> events)
    //{
    //    var appointments = new ObservableCollection<SchedulerAppointment>();
    //    foreach (var ev in events)
    //    {
    //        appointments.Add(new SchedulerAppointment
    //        {
    //            Subject = ev.Name,
    //            StartTime = ev.Date,
    //            EndTime = ev.Date.AddHours(2),
    //        });
    //    }
    //    return appointments;
    //}
}