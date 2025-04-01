using CommunityToolkit.Mvvm.ComponentModel;
using EventPlannerApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using EventPlannerApp.Application.Interfaces;

namespace EventPlannerApp.Presentation.ViewModels
{
    public partial class ModifyEventViewModel : ObservableObject
    {
        [ObservableProperty]
        private Event? myEvent;

        public IRelayCommand<Event?> SaveEventCommand => new RelayCommand<Event?>(SaveEvent);
        public IRelayCommand<Event?> DeleteEventCommand => new RelayCommand<Event?>(DeleteEvent);


        private readonly IServiceProvider _serviceProvider;
        public ModifyEventViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private async void SaveEvent(Event? myEvent)
        {
            if (myEvent == null)
            {
                Console.WriteLine("Event is NULL");
            }
            try
            {
                var eventService = _serviceProvider.GetRequiredService<IEventService>();
                await eventService.UpdateEvent(MyEvent);
                await App.Current.MainPage.DisplayAlert("Erfolgreich", "Event erfolgreich bearbeitet!", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Fehler", $"Event konnte nicht bearbeitet werden!\n{ex}", "OK");
            }
        }

        private async void DeleteEvent(Event? myEvent)
        {
            if (myEvent == null)
            {
                Console.WriteLine("Event is NULL");
            }
            try
            {
                var eventService = _serviceProvider.GetRequiredService<IEventService>();
                await eventService.DeleteEvent(MyEvent.Id);
                await App.Current.MainPage.DisplayAlert("Erfolgreich", "Event erfolgreich gelöscht!", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Fehler", $"Event konnte nicht gelöscht werden!\n{ex}", "OK");
            }
        }
    }
}
