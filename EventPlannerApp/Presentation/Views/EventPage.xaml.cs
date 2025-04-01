using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Presentation.ViewModels;

namespace EventPlannerApp.Presentation;

public partial class EventPage : ContentPage
{
	private readonly IServiceProvider _serviceProvider;
    public EventPage(IServiceProvider serviceProvider)
	{
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }
}