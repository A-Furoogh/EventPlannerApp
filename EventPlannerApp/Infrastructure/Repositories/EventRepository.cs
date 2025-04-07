using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Presentation;
using EventPlannerApp.Presentation.ViewModels;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly FirebaseClient _firebaseClient;
        public ObservableCollection<Event> Events { get; private set; } = [];
        public ObservableCollection<Event> UserEvents { get; private set; } = [];
        public ObservableCollection<Event> PublicEvents { get; private set; } = [];
        public EventRepository(FirebaseClient firebaseClient) 
        {
            _firebaseClient = firebaseClient;
            InitializeRealTimeUpdates();
            LoadData();
        }
        private void LoadData()
        {
            Events = new ObservableCollection<Event>(_firebaseClient
                .Child("Events")
                .OnceAsync<Event>()
                .Result
                .Select(f => f.Object));

            UserEvents = new ObservableCollection<Event>(_firebaseClient
                .Child("Events")
                .OnceAsync<Event>()
                .Result
                .Select(f => f.Object)
                .Where(e => e.ParticipantsIds != null && e.ParticipantsIds.Contains(MainViewModel.UserId)));

            PublicEvents = new ObservableCollection<Event>(_firebaseClient
                .Child("Events")
                .OnceAsync<Event>()
                .Result
                .Select(f => f.Object)
                .Where(e => e.IsPublic));
        }
        private void InitializeRealTimeUpdates()
        {
            _firebaseClient
                .Child("Events")
                .AsObservable<Event>()
                .Subscribe(update =>
                {
                    var ev = update.Object;
                    switch (update.EventType)
                    {
                        case Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate:
                            AddOrUpdateEvent(ev);
                            break;
                        case Firebase.Database.Streaming.FirebaseEventType.Delete:
                            RemoveEvent(ev.Id);
                            break;
                    }
                });
        }
        private void AddOrUpdateEvent(Event ev)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var existingEvent = Events.FirstOrDefault(e => e.Id == ev.Id);
                    if (existingEvent != null)
                    {
                        var index = Events.IndexOf(existingEvent);
                        Events[index] = ev;
                    }
                    else
                    {
                        Events.Add(ev);
                    }

                    var existingUserEvent = UserEvents.FirstOrDefault(e => e.Id == ev.Id);
                    bool isUserPartOfEvent = ev.ParticipantsIds?.Contains(MainViewModel .UserId) ?? false;

                    if (isUserPartOfEvent)
                    {
                        if (existingUserEvent != null)
                        {
                            var userIndex = UserEvents.IndexOf(existingUserEvent);
                            UserEvents[userIndex] = ev;
                        }
                        else
                        {
                            UserEvents.Add(ev);
                        }
                    }
                    else if (existingUserEvent != null)
                    {
                        UserEvents.Remove(existingUserEvent);
                    }

                    var existingPublicEvent = PublicEvents.FirstOrDefault(e => e.Id == ev.Id);
                    if (ev.IsPublic)
                    {
                        if (existingPublicEvent != null)
                        {
                            var publicIndex = PublicEvents.IndexOf(existingPublicEvent);
                            PublicEvents[publicIndex] = ev;
                        }
                        else
                        {
                            PublicEvents.Add(ev);
                        }
                    }
                    else if (existingPublicEvent != null)
                    {
                        PublicEvents.Remove(existingPublicEvent);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating collections: {ex.Message}");
                }
            });
        }
        private void RemoveEvent(int eventId)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var existingEvent = Events.FirstOrDefault(e => e.Id == eventId);
                    if (existingEvent != null) Events.Remove(existingEvent);

                    var existingUserEvent = UserEvents.FirstOrDefault(e => e.Id == eventId);
                    if (existingUserEvent != null) UserEvents.Remove(existingUserEvent);

                    var existingPublicEvent = PublicEvents.FirstOrDefault(e => e.Id == eventId);
                    if (existingPublicEvent != null) PublicEvents.Remove(existingPublicEvent);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error removing event: {ex.Message}");
                }
            });
        }
        public async Task<Event> GetEventById(int eventId)
        {
            return await _firebaseClient.Child("Events").Child(eventId.ToString()).OnceSingleAsync<Event>();
        }
        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            return await Task.FromResult(Events);
        }
        public async Task CreateEvent(Event newEvent)
        {
            await _firebaseClient.Child("Events").Child(newEvent.Id.ToString()).PutAsync(newEvent);
        }
        public async Task UpdateEvent(Event newEvent)
        {
            await _firebaseClient.Child("Events").Child(newEvent.Id.ToString()).PutAsync(newEvent);
            AddOrUpdateEvent(newEvent);
        }
        public async Task DeleteEvent(int eventId)
        {
            await _firebaseClient.Child("Events").Child(eventId.ToString()).DeleteAsync();
        }
        public async Task<IEnumerable<Event>> GetAllEventsByUserId(int userId)
        {
            var events = Events.Where(e => e.OrganizerId.Equals(userId));
            return await Task.FromResult(events);
        }
        public async Task AddUsersToEvent(int eventId, List<int> userIds)
        {
            var ev = await GetEventById(eventId);
            ev.ParticipantsIds?.AddRange(userIds);
            await UpdateEvent(ev);
        }
        public async Task AddUserToEvent(int eventId, int userId)
        {
            // Get event from Result
            var ev = await GetEventById(eventId);
            ev.ParticipantsIds?.Add(userId);
            await UpdateEvent(ev);
        }
        public async Task AddFeedback(Feedback feedback)
        {
            var ev = await GetEventById(feedback.EventId);
            if (ev.Feedbacks == null)
            {
                ev.Feedbacks = new Dictionary<string, Feedback>();
            }
            try
            {
                //await _firebaseClient.Child("Events").Child(feedback.EventId.ToString()).Child("Feedbacks").PostAsync(feedback);
                var response = await _firebaseClient.Child("Events")
                     .Child(feedback.EventId.ToString())
                     .Child("Feedbacks").PostAsync(feedback);

                ev.Feedbacks[response.Key] = feedback;

                await UpdateEvent(ev);
                //AddOrUpdateEvent(ev);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task IncrementViewCount(int eventId)
        {
            try
            {
                var ev = await GetEventById(eventId);
                ev.ViewCount ??= 0;

                ev.ViewCount++;
                _ = UpdateEvent(ev);
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
