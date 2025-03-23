using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Event> GetEventById(int eventId)
        {
            if (eventId <= 0)
            {
                throw new ArgumentException("EventId muss größer als 0 sein");
            }
            if (await _eventRepository.GetEventById(eventId) == null)
            {
                throw new ArgumentException("Event mit dieser Id existiert nicht");
            }
            return await _eventRepository.GetEventById(eventId);
        }

        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            if (await _eventRepository.GetAllEvents() == null)
            {
                throw new ArgumentException("Keine Events vorhanden");
            }
            return await _eventRepository.GetAllEvents();
        }

        public async Task CreateEvent(Event newEvent)
        {
            if (newEvent == null)
            {
                throw new ArgumentException("Event kann nicht null sein");
            }
            if (newEvent.OrganizerId == null)
            {
                throw new ArgumentException("OrganizerId kann nicht null sein");
            }

            if (newEvent.Date < DateTime.Now)
            {
                throw new ArgumentException("Event Datum kann nicht in Vergangenheit sein");
            }

            if (newEvent.EndTime < newEvent.Date)
            {
                throw new ArgumentException("Endzeit kann nicht vor Startzeit sein");
            }

            if (newEvent.Name == null)
            {
                throw new ArgumentException("Name kann nicht null sein");
            }
            await _eventRepository.CreateEvent(newEvent);
        }

        public async Task UpdateEvent(Event newEvent)
        {

            if (newEvent.OrganizerId == null)
            {
                throw new ArgumentException("OrganizerId kann nicht null sein");
            }

            if (newEvent.Date < DateTime.Now)
            {
                throw new ArgumentException("Event Datum kann nicht in Vergangenheit sein");
            }

            if (newEvent.EndTime < newEvent.Date)
            {
                throw new ArgumentException("Endzeit kann nicht vor Startzeit sein");
            }

            if (newEvent.Name == null)
            {
                throw new ArgumentException("Name kann nicht null sein");
            }

            await _eventRepository.UpdateEvent(newEvent);
        }

        public async Task DeleteEvent(int eventId)
        {
            if (eventId <= 0)
            {
                throw new ArgumentException("EventId muss größer als 0 sein");
            }

            if (await _eventRepository.GetEventById(eventId) == null)
            {
                throw new ArgumentException("Event mit dieser Id existiert nicht");
            }
            await _eventRepository.DeleteEvent(eventId);
        }
        public async Task<IEnumerable<Event>> GetAllEventsByUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("UserId muss größer als 0 sein");
            }

            if (await _eventRepository.GetAllEventsByUserId(userId) == null)
            {
                throw new ArgumentException("User hat keine Events");
            }
            return await _eventRepository.GetAllEventsByUserId(userId);
        }
        public async Task AddUsersToEvent(int eventId, List<int> userIds)
        {
            if (eventId <= 0)
            {
                throw new ArgumentException("EventId muss größer als 0 sein");
            }
            if (userIds == null)
            {
                throw new ArgumentException("UserIds kann nicht null sein");
            }
            if (userIds.Count == 0)
            {
                throw new ArgumentException("UserIds kann nicht leer sein");
            }
            await _eventRepository.AddUsersToEvent(eventId, userIds);
        }
        public async Task AddUserToEvent(int eventId, int userId)
        {
            if (eventId <= 0)
            {
                throw new ArgumentException("EventId muss größer als 0 sein");
            }
            if (userId <= 0)
            {
                throw new ArgumentException("UserId muss größer als 0 sein");
            }

            await _eventRepository.AddUserToEvent(eventId, userId);
        }
        public ObservableCollection<Event> Events => _eventRepository.Events;
        public ObservableCollection<Event> UserEvents => _eventRepository.UserEvents;
        public ObservableCollection<Event> PublicEvents => _eventRepository.PublicEvents;
        public async Task AddFeedback(Feedback feedback)
        {
            if (feedback.UserId <= 0)
            {
                throw new ArgumentException("UserId muss größer als 0 sein");
            }
            if (feedback.EventId <= 0)
            {
                throw new ArgumentException("EventId muss größer als 0 sein");
            }
            if (feedback.Rating < 1 || feedback.Rating > 5)
            {
                throw new ArgumentException("Rating muss zwischen 1 und 5 sein");
            }

            await _eventRepository.AddFeedback(feedback);
        }
        public async Task IncrementViewCount(int eventId)
        {
            if (eventId <= 0)
            {
                throw new ArgumentException("EventId muss größer als 0 sein");
            }

            await _eventRepository.IncrementViewCount(eventId);
        }
    }
}
