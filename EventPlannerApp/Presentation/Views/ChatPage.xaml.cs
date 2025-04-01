using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using System.Collections.ObjectModel;

namespace EventPlannerApp.Presentation;

public partial class ChatPage : ContentPage
{
	private readonly IServiceProvider _serviceProvider;
    public ChatPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }
}