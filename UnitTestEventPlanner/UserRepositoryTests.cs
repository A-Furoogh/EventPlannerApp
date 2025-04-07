using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestEventPlanner
{
    public class UserRepositoryTests
    {
        private readonly IUserRepository _fakeUserRepository;

        public UserRepositoryTests()
        {
            _fakeUserRepository = A.Fake<IUserRepository>();
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User { Id = userId, Name = "Test User", Password = "www" };
            // Set up the fake behavior
            A.CallTo(() => _fakeUserRepository.GetUserByIdAsync(userId))
                .Returns(expectedUser);
            // Act
            var result = await _fakeUserRepository.GetUserByIdAsync(userId);
            
            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.Name.Should().Be("Test User");

            // Verify the method was called
            A.CallTo(() => _fakeUserRepository.GetUserByIdAsync(userId))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User { Id = 1, Name = "User 1", Password = "www" },
                new User { Id = 2, Name = "User 2", Password = "www" }
            };
            // Set up the fake behavior
            A.CallTo(() => _fakeUserRepository.GetAllUsersAsync())
                .Returns(expectedUsers);
            // Act
            var result = await _fakeUserRepository.GetAllUsersAsync();
            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.First().Id.Should().Be(1);
            result.First().Name.Should().Be("User 1");
            result.Last().Id.Should().Be(2);
            result.Last().Name.Should().Be("User 2");
            // Verify the method was called
            A.CallTo(() => _fakeUserRepository.GetAllUsersAsync())
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddUser_ShouldAddUser_WhenUserDoesNotExist()
        {
            // Arrange
            var newUser = new User { Id = 3, Name = "User 3", Password = "www" };
            // Set up the fake behavior
            A.CallTo(() => _fakeUserRepository.AddUserAsync(newUser))
                .DoesNothing();
            // Act
            await _fakeUserRepository.AddUserAsync(newUser);
            // Verify the method was called
            A.CallTo(() => _fakeUserRepository.AddUserAsync(newUser))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddUser_ShouldNotAddUser_WhenUserExists()
        {
            // Arrange
            var existingUser = new User { Id = 1, Name = "User 1", Password = "www" };

            // Simulate that the user already exists
            A.CallTo(() => _fakeUserRepository.GetUserByIdAsync(existingUser.Id))
                .Returns(existingUser);

            // Act
            // Simulate the logic that prevents AddUserAsync from being called
            var user = await _fakeUserRepository.GetUserByIdAsync(existingUser.Id);
            if (user != null)
            {
                // If the user exists, AddUserAsync should not be called
                A.CallTo(() => _fakeUserRepository.AddUserAsync(existingUser))
                    .MustNotHaveHappened();
            }
            else
            {
                // If the user does not exist, AddUserAsync should be called
                await _fakeUserRepository.AddUserAsync(existingUser);
            }

            // Assert
            // Verify that AddUserAsync was not called
            A.CallTo(() => _fakeUserRepository.AddUserAsync(existingUser))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task UpdateUser_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var userToUpdate = new User { Id = userId, Name = "User 1", Password = "www" };
            // Set up the fake behavior
            A.CallTo(() => _fakeUserRepository.UpdateUserAsync(userToUpdate))
                .DoesNothing();
            // Act
            await _fakeUserRepository.UpdateUserAsync(userToUpdate);
            // Verify the method was called
            A.CallTo(() => _fakeUserRepository.UpdateUserAsync(userToUpdate))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateUser_ShouldNotUpdateUser_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var userToUpdate = new User { Id = userId, Name = "User 1", Password = "www" };

            A.CallTo(() => _fakeUserRepository.GetUserByIdAsync(userId))
                .Returns(Task.FromResult<User>(null));
            // Act
            var user = await _fakeUserRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                await _fakeUserRepository.UpdateUserAsync(userToUpdate);
            }
            else
            {
                A.CallTo(() => _fakeUserRepository.UpdateUserAsync(userToUpdate))
                    .MustNotHaveHappened();
            }
            // Assert
            A.CallTo(() => _fakeUserRepository.UpdateUserAsync(userToUpdate))
                .MustNotHaveHappened();
        }
    }
}
