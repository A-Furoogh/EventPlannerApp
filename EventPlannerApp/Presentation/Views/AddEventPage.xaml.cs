using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Infrastructure.Repositories;

namespace EventPlannerApp.Presentation;

public partial class AddEventPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    public AddEventPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }
}