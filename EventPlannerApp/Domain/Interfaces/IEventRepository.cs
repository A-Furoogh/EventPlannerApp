using EventPlannerApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Application.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEvents();
        Task<Event> GetEventById(int id);
        Task CreateEvent(Event newEvent);
        Task UpdateEvent(Event updatedEvent);
        Task DeleteEvent(int id);
        Task<IEnumerable<Event>> GetAllEventsByUserId(int userId);
        Task AddUsersToEvent(int eventId, List<int> userIds);
        Task AddUserToEvent(int eventId, int userId);
        ObservableCollection<Event> Events { get; }
        ObservableCollection<Event> UserEvents { get; }
        ObservableCollection<Event> PublicEvents { get; }
        Task AddFeedback(Feedback feedback);
        Task IncrementViewCount(int eventId);
    }
}
