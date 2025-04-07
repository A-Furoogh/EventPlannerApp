using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Infrastructure.Helpers;
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
    public class UserRepository : IUserRepository
    {
        private readonly FirebaseClient _firebaseClient;
        public ObservableCollection<User> Users { get; private set; } = new ObservableCollection<User>();
        public UserRepository(FirebaseClient firebaseClient)
        {
            _firebaseClient = firebaseClient;
            InitializeRealTimeUpdates();
            LoadData();
        }
        private void LoadData()
        {
            var users = _firebaseClient.Child("Users").OnceAsync<User>().Result;
            Users = new ObservableCollection<User>(users.Select(u => u.Object));
        }
        private void InitializeRealTimeUpdates()
        {
            _firebaseClient
                .Child("Users")
                .AsObservable<User>()
                .Subscribe(update =>
                {   
                    var user = update.Object;
                    switch (update.EventType)
                    {
                        case Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate:
                            AddOrUpdateUser(user);
                            break;
                        case Firebase.Database.Streaming.FirebaseEventType.Delete:
                            RemoveUser(user.Id);
                            break;
                    }
                });
        }
        private void AddOrUpdateUser(User user)
        {
            var existingUser = Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                // Update existing user
                var index = Users.IndexOf(existingUser);
                Users[index] = user;
            }
            else
            {
                // Add new user
                Users.Add(user);
            }
        }

        public void RemoveUser(int userId)
        {
            var userToRemove = Users.FirstOrDefault(u => u.Id == userId);
            if (userToRemove != null)
            {
                Users.Remove(userToRemove);
            }
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _firebaseClient.Child("Users").Child(userId.ToString()).OnceSingleAsync<User>();
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _firebaseClient.Child("Users").OnceAsync<User>();
            return users.Select(u => u.Object);
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                var users = await _firebaseClient.Child("Users").OnceAsync<User>();
                if (users.Any(u => u.Object.Name == user.Name))
                {
                    throw new Exception("Benutzername ist schon benutzt! Versuchen Sie mit einem anderen Benutzernamen");
                }
                var hashedPassword = PasswordHelper.HashPassword(user.Password);
                user.Password = hashedPassword;
                await _firebaseClient.Child("Users").Child(user.Id.ToString()).PutAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            await _firebaseClient.Child("Users").Child(user.Id.ToString()).PutAsync(user);
        }

        public async Task DeleteUserAsync(String userId)
        {
            await _firebaseClient.Child("Users").Child(userId).DeleteAsync();
        }
    }
}
