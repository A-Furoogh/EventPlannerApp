using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Infrastructure.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly FirebaseClient _firebaseClient;
        private int _userId;
        public AuthenticationRepository(FirebaseClient firebaseClient)
        {
            _firebaseClient = firebaseClient;
        }
        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            var users = await _firebaseClient.Child("Users").OnceAsync<User>();
            var user = users.Where(u => u.Object.Name.ToLower() == username.ToLower()).FirstOrDefault()?.Object;
            if (user != null && user.Password == password)
            {
                _userId = user.Id;
                return true;
            }
            return false;
        }
        public int GetUserIdAsync()
        {
            return _userId;
        }
    }
}
