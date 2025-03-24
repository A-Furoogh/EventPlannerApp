using EventPlannerApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private bool _isAuthenticated;
        public AuthenticationService(IAuthenticationRepository authenticationService)
        {
            _authenticationRepository = authenticationService;
        }
        public bool IsUserAuthenticated()
        {
            return _isAuthenticated;
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Username und Passwort dürfen nicht leer sein");
            }
            bool isAuthenticated = await _authenticationRepository.AuthenticateUserAsync(username, password);
            if (isAuthenticated)
            {
                _isAuthenticated = true;
                return true;
            }
            else
            {
                _isAuthenticated = false;
                return false;
            }
        }
        public int GetUserIdAsync()
        {
            return _authenticationRepository.GetUserIdAsync();
        }

        public void Logout()
        {
            Preferences.Default.Remove("Username");
            Preferences.Default.Remove("Password");
            _isAuthenticated = false;
        }
    }
}
