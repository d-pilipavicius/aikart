using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using aiKart.Controllers;
using aiKart.Interfaces;
using aiKart.Models;
using aiKart.Dtos.DeckDtos;
using aiKart.Dtos.CardDtos;
using aiKart.States;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace aiKart.Tests
{
    public class DeckControllerTests
    {
        private readonly Mock<IDeckService> mockDeckService;
        private readonly Mock<IMapper> mockMapper;
        private readonly DeckController controller;

        public DeckControllerTests()
        {
            mockDeckService = new Mock<IDeckService>();
            mockMapper = new Mock<IMapper>();
            controller = new DeckController(mockDeckService.Object, mockMapper.Object);
        }

        [Fact]
        public void GetAllDecks_ReturnsOk()
        {
            mockDeckService.Setup(s => s.GetAllDecksIncludingCards()).Returns(new List<Deck>());
            mockMapper.Setup(m => m.Map<List<DeckDto>>(It.IsAny<List<Deck>>()))
                .Returns(new List<DeckDto> { new DeckDto(1, "Name", null, null, DateTime.Now, DateTime.Now, null) });
            var result = controller.GetAllDecks();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetDeck_ReturnsNotFound()
        {
            mockDeckService.Setup(s => s.DeckExistsById(It.IsAny<int>())).Returns(false);
            var result = controller.GetDeck(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetDeck_ReturnsOk()
        {
            mockDeckService.Setup(s => s.DeckExistsById(It.IsAny<int>())).Returns(true);
            mockDeckService.Setup(s => s.GetDeckById(It.IsAny<int>())).Returns(new Deck());
            mockMapper.Setup(m => m.Map<DeckDto>(It.IsAny<Deck>()))
                .Returns(new DeckDto(1, "Name", null, null, DateTime.Now, DateTime.Now, null));
            var result = controller.GetDeck(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCardsInDeck_ReturnsNotFound()
        {
            mockDeckService.Setup(s => s.DeckExistsById(It.IsAny<int>())).Returns(false);
            var result = controller.GetCardsInDeck(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetCardsInDeck_ReturnsOk()
        {
            mockDeckService.Setup(s => s.DeckExistsById(It.IsAny<int>())).Returns(true);
            mockDeckService.Setup(s => s.GetCardsInDeck(It.IsAny<int>())).Returns(new List<Card>());
            mockMapper.Setup(m => m.Map<IEnumerable<CardDto>>(It.IsAny<IEnumerable<Card>>()))
                .Returns(new List<CardDto> { new CardDto(1, 1, "Question", "Answer", CardState.Answered) });
            var result = controller.GetCardsInDeck(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddDeck_ReturnsOk()
        {
            mockDeckService.Setup(s => s.DeckExistsByName(It.IsAny<string>())).Returns(false);
            mockDeckService.Setup(s => s.AddDeck(It.IsAny<Deck>())).Returns(true);
            var result = controller.AddDeck(new AddDeckDto("Name", null, null));
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateDeck_ReturnsNotFound()
        {
            mockDeckService.Setup(s => s.DeckExistsById(It.IsAny<int>())).Returns(false);
            var result = controller.UpdateDeck(1, new UpdateDeckDto("Name", null, null));
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateDeck_ReturnsNoContent()
        {
            mockDeckService.Setup(s => s.DeckExistsById(It.IsAny<int>())).Returns(true);
            mockDeckService.Setup(s => s.GetDeckById(It.IsAny<int>())).Returns(new Deck());
            mockDeckService.Setup(s => s.UpdateDeck(It.IsAny<Deck>())).Returns(true);
            var result = controller.UpdateDeck(1, new UpdateDeckDto("Name", null, null));
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteDeck_ReturnsNotFound()
        {
            mockDeckService.Setup(s => s.DeckExistsById(It.IsAny<int>())).Returns(false);
            var result = controller.DeleteDeck(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteDeck_ReturnsNoContent()
        {
            mockDeckService.Setup(s => s.DeckExistsById(It.IsAny<int>())).Returns(true);
            mockDeckService.Setup(s => s.GetDeckById(It.IsAny<int>())).Returns(new Deck());
            mockDeckService.Setup(s => s.DeleteDeck(It.IsAny<Deck>())).Returns(true);
            var result = controller.DeleteDeck(1);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
