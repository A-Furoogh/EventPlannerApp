using System.Collections.ObjectModel;

namespace EventPlannerApp.Presentation;

public partial class EventImagePage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    public EventImagePage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _serviceProvider = serviceProvider;
    }
}