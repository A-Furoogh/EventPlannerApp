using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;

namespace EventPlannerApp.Presentation;

public partial class ModifyEventPage : ContentPage
{
	private readonly IServiceProvider _serviceProvider;
    public ModifyEventPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }
}