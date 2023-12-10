using Xunit;
using Microsoft.EntityFrameworkCore;
using aiKart.Data;
using aiKart.Models;
using aiKart.Repositories;
using System;
using System.Linq;

namespace aiKart.Tests
{
    public class UserRepositoryTests
    {
        private readonly DataContext _context;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            string uniqueDatabaseName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: uniqueDatabaseName)
                .Options;

            _context = new DataContext(options);

            // Seed with some User data
            _context.Users.AddRange(
                new User { Id = 1, Name = "Alice" },
                new User { Id = 2, Name = "Bob" }
            );
            _context.SaveChanges();

            _userRepository = new UserRepository(_context);
        }

        [Fact]
        public void GetUsers_ShouldReturnAllUsers()
        {
            var result = _userRepository.GetUsers();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetUser_ById_UserExists_ReturnsUser()
        {
            var result = _userRepository.GetUser(1);
            Assert.NotNull(result);
            Assert.Equal("Alice", result.Name);
        }

        [Fact]
        public void GetUser_ById_UserDoesNotExist_ReturnsNull()
        {
            var result = _userRepository.GetUser(999);
            Assert.Null(result);
        }

        [Fact]
        public void GetUser_ByName_UserExists_ReturnsUser()
        {
            var result = _userRepository.GetUser("Alice");
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetUser_ByName_UserDoesNotExist_ReturnsNull()
        {
            var result = _userRepository.GetUser("Charlie");
            Assert.Null(result);
        }

        [Fact]
        public void UserExists_ById_UserExists_ReturnsTrue()
        {
            bool exists = _userRepository.UserExists(1);
            Assert.True(exists);
        }

        [Fact]
        public void UserExists_ById_UserDoesNotExist_ReturnsFalse()
        {
            bool exists = _userRepository.UserExists(999);
            Assert.False(exists);
        }

        [Fact]
        public void UserExists_ByName_UserExists_ReturnsTrue()
        {
            bool exists = _userRepository.UserExists("Alice");
            Assert.True(exists);
        }

        [Fact]
        public void UserExists_ByName_UserDoesNotExist_ReturnsFalse()
        {
            bool exists = _userRepository.UserExists("Charlie");
            Assert.False(exists);
        }

        [Fact]
        public void AddUser_ValidUser_ReturnsTrue()
        {
            var newUser = new User { Id = 3, Name = "Charlie" };
            bool result = _userRepository.AddUser(newUser);

            Assert.True(result);
            Assert.Equal(3, _context.Users.Count());
        }

    }
}
