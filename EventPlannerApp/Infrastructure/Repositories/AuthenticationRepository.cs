using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Infrastructure.Helpers;
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
        public AuthenticationRepository(FirebaseClient firebaseClient)
        {
            _firebaseClient = firebaseClient;
        }
        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            var users = await _firebaseClient.Child("Users").OnceAsync<User>();
            var user = users.Where(u => u.Object.Name.ToLower() == username.ToLower()).FirstOrDefault()?.Object;
            if (user != null)
            {
                try
                {
                    var hashedPassword = user.Password;
                    if (PasswordHelper.VerifyPassword(password, hashedPassword))
                    {
                        Preferences.Default.Set("userId", user.Id);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception("Einloggen Fehlgeschlagen", ex);
                }
            }
            return false;
        }
        public int GetUserIdAsync()
        {
            var id = Preferences.Default.Get("userId", 0);
            return id;
        }
    }
}
