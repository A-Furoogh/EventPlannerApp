using System.Collections.ObjectModel;
using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;

namespace EventPlannerApp.Presentation;

public partial class AnalyticsPage : ContentPage
{
	private readonly IServiceProvider _serviceProvider;
    public AnalyticsPage(IServiceProvider serviceProvider)
	{
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }
}