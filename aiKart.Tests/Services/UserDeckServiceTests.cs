using Xunit;
using Moq;
using aiKart.Models;
using aiKart.Services;
using aiKart.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace aiKart.Tests
{
    public class UserDeckServiceTests
    {
        private readonly Mock<IUserDeckRepository> _mockUserDeckRepository;
        private readonly UserDeckService _userDeckService;

        public UserDeckServiceTests()
        {
            _mockUserDeckRepository = new Mock<IUserDeckRepository>();
            _userDeckService = new UserDeckService(_mockUserDeckRepository.Object);
        }

        [Fact]
        public void GetUserDecks_ShouldReturnAllUserDecks()
        {
            var userDecks = new List<UserDeck>
            {
                new UserDeck { UserId = 1, DeckId = 1 },
                new UserDeck { UserId = 2, DeckId = 2 }
            };

            _mockUserDeckRepository.Setup(repo => repo.GetUserDecks()).Returns(userDecks);

            var result = _userDeckService.GetUserDecks();

            Assert.Equal(2, result.Count);
            _mockUserDeckRepository.Verify(repo => repo.GetUserDecks(), Times.Once);
        }

        [Fact]
        public void GetDecksByUser_ShouldReturnDecksForSpecificUser()
        {
            var decks = new List<Deck?> { new Deck { Id = 1 }, new Deck { Id = 2 } };

            _mockUserDeckRepository.Setup(repo => repo.GetDecksByUser(It.IsAny<int>())).Returns(decks);

            var result = _userDeckService.GetDecksByUser(1);

            Assert.Equal(2, result.Count);
            _mockUserDeckRepository.Verify(repo => repo.GetDecksByUser(1), Times.Once);
        }

        [Fact]
        public void GetUsersOfADeck_ShouldReturnUsersForSpecificDeck()
        {
            var users = new List<User?> { new User { Id = 1 }, new User { Id = 2 } };

            _mockUserDeckRepository.Setup(repo => repo.GetUsersOfADeck(It.IsAny<int>())).Returns(users);

            var result = _userDeckService.GetUsersOfADeck(1);

            Assert.Equal(2, result.Count);
            _mockUserDeckRepository.Verify(repo => repo.GetUsersOfADeck(1), Times.Once);
        }

        [Fact]
        public void UserDeckExists_ShouldReturnTrueIfExists()
        {
            _mockUserDeckRepository.Setup(repo => repo.UserDeckExists(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            bool exists = _userDeckService.UserDeckExists(1, 1);

            Assert.True(exists);
            _mockUserDeckRepository.Verify(repo => repo.UserDeckExists(1, 1), Times.Once);
        }

        [Fact]
        public void AddUserDeck_ShouldAddUserDeck()
        {
            _mockUserDeckRepository.Setup(repo => repo.AddUserDeck(It.IsAny<UserDeck>())).Returns(true);

            var userDeck = new UserDeck { UserId = 1, DeckId = 1 };
            bool result = _userDeckService.AddUserDeck(userDeck);

            Assert.True(result);
            _mockUserDeckRepository.Verify(repo => repo.AddUserDeck(userDeck), Times.Once);
        }

        [Fact]
        public void DeleteUserDeck_ShouldDeleteUserDeck()
        {
            _mockUserDeckRepository.Setup(repo => repo.GetUserDecks()).Returns(new List<UserDeck> { new UserDeck { UserId = 1, DeckId = 1 } });
            _mockUserDeckRepository.Setup(repo => repo.DeleteUserDeck(It.IsAny<UserDeck>())).Returns(true);

            bool result = _userDeckService.DeleteUserDeck(1, 1);

            Assert.True(result);
            _mockUserDeckRepository.Verify(repo => repo.DeleteUserDeck(It.IsAny<UserDeck>()), Times.Once);
        }

        [Fact]
        public void Save_ShouldReturnTrueOnSuccess()
        {
            _mockUserDeckRepository.Setup(repo => repo.Save()).Returns(true);

            bool result = _userDeckService.Save();

            Assert.True(result);
            _mockUserDeckRepository.Verify(repo => repo.Save(), Times.Once);
        }
    }
}
