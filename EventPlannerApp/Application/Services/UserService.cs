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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Ungültige user ID");
            }
            if (_userRepository.Users.All(u => u.Id != userId))
            {
                throw new KeyNotFoundException("User nicht gefunden");
            }
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            if (_userRepository.Users.Count == 0)
            {
                throw new KeyNotFoundException("Keine User gefunden");
            }

            return await _userRepository.GetAllUsersAsync();
        }

        public async Task AddUserAsync(User user)
        {
            if (user == null) {
                throw new ArgumentNullException("User darf nicht null sein");
            }
            if (user.Id <= 0)
            {
                throw new ArgumentException("Ungültige user ID");
            }
            if (_userRepository.Users.Any(u => u.Id == user.Id))
            {
                throw new ArgumentException("User ID bereits vorhanden");
            }
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User darf nicht null sein");
            }
            if (user.Id <= 0)
            {
                throw new ArgumentException("Ungültige user ID");
            }
            if (_userRepository.Users.All(u => u.Id != user.Id))
            {
                throw new KeyNotFoundException("User nicht gefunden");
            }
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(String userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("User ID darf nicht null sein");
            }
            await _userRepository.DeleteUserAsync(userId);
        }

        public ObservableCollection<User> Users => _userRepository.Users;
    }
}
