using CommunityToolkit.Mvvm.ComponentModel;
using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Presentation.ViewModels
{
    public partial class AnalyticsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Event>? _userEvents;

        private readonly IServiceProvider _serviceProvider;
        public AnalyticsViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadData();
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
}
