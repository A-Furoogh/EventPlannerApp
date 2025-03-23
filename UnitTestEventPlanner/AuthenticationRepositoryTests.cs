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
    public class AuthenticationRepositoryTests
    {
        private readonly IAuthenticationRepository _fakeAuthenticationRepository;

        public AuthenticationRepositoryTests()
        {
            _fakeAuthenticationRepository = A.Fake<IAuthenticationRepository>();
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var username = "test";
            var password = "test";
            var expectedUser = new User { Id = 1, Name = username, Password = password };
            // Set up the fake behavior
            A.CallTo(() => _fakeAuthenticationRepository.AuthenticateUserAsync(username, password))
                .Returns(true);
            // Act
            var result = await _fakeAuthenticationRepository.AuthenticateUserAsync(username, password);
            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GetUserIdAsync_ShouldReturnUserId()
        {
            // Arrange
            var expectedUserId = 1;
            // Set up the fake behavior
            A.CallTo(() => _fakeAuthenticationRepository.GetUserIdAsync())
                .Returns(expectedUserId);
            // Act
            var result = _fakeAuthenticationRepository.GetUserIdAsync();
            // Assert
            result.Should().Be(expectedUserId);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "test";
            var password = "test";

            var expectedUser = new User { Id = 1, Name = username, Password = password };
            // Set up the fake behavior
            A.CallTo(() => _fakeAuthenticationRepository.AuthenticateUserAsync(username, password))
                .Returns(false);
            // Act
            var result = await _fakeAuthenticationRepository.AuthenticateUserAsync(username, password);
            // Assert
            result.Should().BeFalse();

        }
        }
    }
