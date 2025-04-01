using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Presentation.ViewModels;
using System.Collections.ObjectModel;

namespace EventPlannerApp.Presentation;

public partial class MyEventsPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public MyEventsPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _serviceProvider = serviceProvider;
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