using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;

namespace EventPlannerApp.Presentation.ViewModels
{
    public partial class MyEventsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Event>? userEvents = new();
        [ObservableProperty]
        private Event? selectedEvent;

        public IRelayCommand<Event?> OnEventSelectedCommand => new RelayCommand<Event?>(OnEventSelected);
        public IRelayCommand<Event?> QrButtonClickedCommand => new RelayCommand<Event?>(OnQrButtonClicked);
        public IRelayCommand OnAnalyticsClickedCommand => new RelayCommand(OnAnalyticsClicked);

        private readonly IServiceProvider _serviceProvider;

        public MyEventsViewModel(IServiceProvider serviceProvider)
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

        private void OnEventSelected(Event? selectedEvent) 
        {
            if (selectedEvent == null)
            {
                Console.WriteLine("Selected Event is NULL");
            }
            else
            {
                Console.WriteLine($"Selected Event Type: {selectedEvent.GetType()}");
            }
            if (selectedEvent != null)
            {
                var eventPage = _serviceProvider.GetRequiredService<EventPage>();
                eventPage.InitializeAsync(selectedEvent);
                var navigationService = _serviceProvider.GetRequiredService<INavigation>();
                navigationService.PushAsync(eventPage);
            }
        }

        private void OnQrButtonClicked(Event? selectedEvent)
        {
            if (selectedEvent != null)
            {
                var qrCodeViewModel = _serviceProvider.GetRequiredService<QrCodeViewModel>();
                qrCodeViewModel.EventId = selectedEvent.Id;
                qrCodeViewModel.GenerateQrCode();
                var navigationService = _serviceProvider.GetRequiredService<INavigation>();
                navigationService.PushAsync(new QrCodePage(qrCodeViewModel));
            }
        }

        private void OnAnalyticsClicked()
        {
            var analyticsPage = _serviceProvider.GetRequiredService<AnalyticsPage>();
            var navigationService = _serviceProvider.GetRequiredService<INavigation>();
            navigationService.PushAsync(analyticsPage);
        }
    }
}
