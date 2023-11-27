using System.Collections.Generic;
using Xunit;
using Moq;
using aiKart.Controllers;
using aiKart.Interfaces;
using aiKart.Models;
using aiKart.Dtos.DeckDtos;
using aiKart.Dtos.UserDtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace aiKart.Tests
{
    public class UserDeckControllerTests
    {
        private readonly Mock<IUserDeckService> mockUserDeckService;
        private readonly Mock<IMapper> mockMapper;
        private readonly UserDeckController userDeckController;

        public UserDeckControllerTests()
        {
            mockUserDeckService = new Mock<IUserDeckService>();
            mockMapper = new Mock<IMapper>();
            userDeckController = new UserDeckController(mockUserDeckService.Object, mockMapper.Object);
        }

        [Fact]
        public void GetUserDecks_ReturnsOk()
        {
            var userDecks = new List<UserDeck> { new UserDeck { UserId = 1, DeckId = 2 } };
            var userDeckDtos = new List<UserDeckDto> { new UserDeckDto(1, 2) };

            mockUserDeckService.Setup(s => s.GetUserDecks()).Returns(userDecks);
            mockMapper.Setup(m => m.Map<List<UserDeckDto>>(userDecks)).Returns(userDeckDtos);

            var result = userDeckController.GetUserDecks();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userDeckDtos, okResult.Value);
        }

        [Fact]
        public void GetDecksByUser_ReturnsNotFound()
        {
            int userId = 1;
            mockUserDeckService.Setup(s => s.GetDecksByUser(userId)).Returns(new List<Deck?>());

            var result = userDeckController.GetDecksByUser(userId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetDecksByUser_ReturnsOk()
        {
            int userId = 1;
            var decks = new List<Deck?> { new Deck { Id = 2, Name = "Test Deck" } };
            var deckDtos = new List<DeckDto> { new DeckDto(2, "Test Deck", 1, null, null, true, default, default, null) };

            mockUserDeckService.Setup(s => s.GetDecksByUser(userId)).Returns(decks);
            mockMapper.Setup(m => m.Map<List<DeckDto>>(decks)).Returns(deckDtos);

            var result = userDeckController.GetDecksByUser(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(deckDtos, okResult.Value);
        }

        [Fact]
        public void GetUsersOfADeck_ReturnsNotFound()
        {
            int deckId = 2;
            mockUserDeckService.Setup(s => s.GetUsersOfADeck(deckId)).Returns(new List<User?>());

            var result = userDeckController.GetUsersOfADeck(deckId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetUsersOfADeck_ReturnsOk()
        {
            int deckId = 2;
            var users = new List<User?> { new User { Id = 1, Name = "Test User" } };
            var userDtos = new List<UserDto> { new UserDto("Test User") };

            mockUserDeckService.Setup(s => s.GetUsersOfADeck(deckId)).Returns(users);
            mockMapper.Setup(m => m.Map<List<UserDto>>(users)).Returns(userDtos);

            var result = userDeckController.GetUsersOfADeck(deckId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userDtos, okResult.Value);
        }

        [Fact]
        public void AddUserDeck_ReturnsNoContent()
        {
            var userDeckDto = new UserDeckDto(1, 2);
            var userDeck = new UserDeck { UserId = 1, DeckId = 2 };

            mockUserDeckService.Setup(s => s.AddUserDeck(It.IsAny<UserDeck>())).Returns(true);
            mockMapper.Setup(m => m.Map<UserDeck>(userDeckDto)).Returns(userDeck);

            var result = userDeckController.AddUserDeck(userDeckDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteUserDeck_ReturnsNoContent()
        {
            int userId = 1, deckId = 2;
            mockUserDeckService.Setup(s => s.UserDeckExists(userId, deckId)).Returns(true);
            mockUserDeckService.Setup(s => s.DeleteUserDeck(userId, deckId)).Returns(true);

            var result = userDeckController.DeleteUserDeck(userId, deckId);

            Assert.IsType<NoContentResult>(result);
        }

    }
}
