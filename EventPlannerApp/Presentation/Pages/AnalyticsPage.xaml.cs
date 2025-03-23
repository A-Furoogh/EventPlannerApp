using System.Collections.ObjectModel;
using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;

namespace EventPlannerApp.Presentation;

public partial class AnalyticsPage : ContentPage
{
	private readonly IServiceProvider _serviceProvider;
    public ObservableCollection<Event>? UserEvents { get; set; } = [];
    public AnalyticsPage(IServiceProvider serviceProvider)
	{
        _serviceProvider = serviceProvider;
        InitializeComponent();
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
    }
}