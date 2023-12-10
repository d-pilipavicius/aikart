using Xunit;
using Moq;
using aiKart.Models;
using aiKart.Services;
using aiKart.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace aiKart.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public void GetUsers_ShouldReturnAllUsers()
        {
            var users = new List<User>
            {
                new User { Id = 1, Name = "Alice" },
                new User { Id = 2, Name = "Bob" }
            };

            _mockUserRepository.Setup(repo => repo.GetUsers()).Returns(users);

            var result = _userService.GetUsers();

            Assert.Equal(2, result.Count());
            _mockUserRepository.Verify(repo => repo.GetUsers(), Times.Once);
        }

        [Fact]
        public void GetUser_ById_ShouldReturnUser()
        {
            var user = new User { Id = 1, Name = "Alice" };

            _mockUserRepository.Setup(repo => repo.GetUser(1)).Returns(user);

            var result = _userService.GetUser(1);

            Assert.NotNull(result);
            Assert.Equal("Alice", result.Name);
            _mockUserRepository.Verify(repo => repo.GetUser(1), Times.Once);
        }

        [Fact]
        public void GetUser_ByName_ShouldReturnUser()
        {
            var user = new User { Id = 1, Name = "Alice" };

            _mockUserRepository.Setup(repo => repo.GetUser("Alice")).Returns(user);

            var result = _userService.GetUser("Alice");

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockUserRepository.Verify(repo => repo.GetUser("Alice"), Times.Once);
        }

        [Fact]
        public void UserExists_ById_ShouldReturnTrueIfUserExists()
        {
            _mockUserRepository.Setup(repo => repo.UserExists(1)).Returns(true);

            bool exists = _userService.UserExists(1);

            Assert.True(exists);
            _mockUserRepository.Verify(repo => repo.UserExists(1), Times.Once);
        }

        [Fact]
        public void UserExists_ByName_ShouldReturnTrueIfUserExists()
        {
            _mockUserRepository.Setup(repo => repo.UserExists("Alice")).Returns(true);

            bool exists = _userService.UserExists("Alice");

            Assert.True(exists);
            _mockUserRepository.Verify(repo => repo.UserExists("Alice"), Times.Once);
        }

        [Fact]
        public void AddUser_ValidUser_ReturnsTrue()
        {
            var newUser = new User { Id = 3, Name = "Charlie" };
            _mockUserRepository.Setup(repo => repo.UserExists("Charlie")).Returns(false);
            _mockUserRepository.Setup(repo => repo.AddUser(newUser)).Returns(true);

            bool result = _userService.AddUser(newUser);

            Assert.True(result);
            _mockUserRepository.Verify(repo => repo.AddUser(newUser), Times.Once);
        }

        [Fact]
        public void AddUser_ExistingUser_ReturnsFalse()
        {
            var existingUser = new User { Id = 1, Name = "Alice" };
            _mockUserRepository.Setup(repo => repo.UserExists("Alice")).Returns(true);

            bool result = _userService.AddUser(existingUser);

            Assert.False(result);
            _mockUserRepository.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Never);
        }

    }
}
