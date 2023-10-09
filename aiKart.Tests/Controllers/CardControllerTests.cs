using System;
using Xunit;
using Moq;
using aiKart.Controllers;
using aiKart.Interfaces;
using aiKart.Models;
using aiKart.Dtos.CardDtos;
using aiKart.States;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace aiKart.Tests.Controllers
{
    public class CardControllerTests
    {
        private readonly CardController _controller;
        private readonly Mock<ICardService> _mockCardService;
        private readonly Mock<IDeckService> _mockDeckService;
        private readonly Mock<IMapper> _mockMapper;

        public CardControllerTests()
        {
            _mockCardService = new Mock<ICardService>();
            _mockDeckService = new Mock<IDeckService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CardController(_mockCardService.Object, _mockDeckService.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetCard_CardExists_ReturnsOk()
        {
            var cardId = 1;
            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);
            _mockCardService.Setup(service => service.GetCardById(cardId)).Returns(new Card());
            _mockMapper.Setup(mapper => mapper.Map<CardDto>(It.IsAny<Card>())).Returns(new CardDto(1, 1, "Question", "Answer", CardState.Answered));

            var result = _controller.GetCard(cardId);

            Assert.IsType<OkObjectResult>(result);

            _mockCardService.Verify(service => service.CardExists(cardId), Times.Once);
            _mockCardService.Verify(service => service.GetCardById(cardId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<CardDto>(It.IsAny<Card>()), Times.Once);
        }

        [Fact]
        public void GetCard_CardDoesNotExist_ReturnsNotFound()
        {
            var cardId = 1;
            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(false);

            var result = _controller.GetCard(cardId);

            Assert.IsType<NotFoundResult>(result);

            _mockCardService.Verify(service => service.CardExists(cardId), Times.Once);
        }

        [Fact]
        public void GetStateTypes_ReturnsOk()
        {
            var result = _controller.GetStateTypes();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteCard_ValidCard_ReturnsNoContent()
        {
            var cardId = 1;
            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);
            _mockCardService.Setup(service => service.GetCardById(cardId)).Returns(new Card());
            _mockCardService.Setup(service => service.DeleteCard(It.IsAny<Card>())).Returns(true);


            var result = _controller.DeleteCard(cardId);

            Assert.IsType<NoContentResult>(result);

            _mockCardService.Verify(service => service.CardExists(cardId), Times.Once);
            _mockCardService.Verify(service => service.GetCardById(cardId), Times.Once);
        }

        [Fact]
        public void AddCard_ValidCard_ReturnsOk()
        {
            var addCardDto = new AddCardDto(1, "Question", "Answer");
            _mockDeckService.Setup(service => service.DeckExistsById(addCardDto.DeckId)).Returns(true);
            _mockCardService.Setup(service => service.AddCardToDeck(It.IsAny<Card>())).Returns(true);
            _mockCardService.Setup(service => service.UpdateCard(It.IsAny<Card>())).Returns(true);


            var result = _controller.AddCard(addCardDto);

            Assert.IsType<OkResult>(result);

            _mockDeckService.Verify(service => service.DeckExistsById(addCardDto.DeckId), Times.Once);
            _mockCardService.Verify(service => service.AddCardToDeck(It.IsAny<Card>()), Times.Once);
        }

        [Fact]
        public void UpdateCard_ValidCard_ReturnsNoContent()
        {
            var cardId = 1;
            var updateCardDto = new UpdateCardDto("Updated Question", "Updated Answer");
            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);
            _mockCardService.Setup(service => service.UpdateCard(It.IsAny<Card>())).Returns(true);


            var result = _controller.UpdateCard(cardId, updateCardDto);

            Assert.IsType<NoContentResult>(result);

            _mockCardService.Verify(service => service.CardExists(cardId), Times.Once);
        }

        [Fact]
        public void SetCardState_ValidCardState_ReturnsNoContent()
        {
            var cardId = 1;
            var cardStateDto = new CardStateDto(CardState.Answered);

            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);

            var card = new Card();
            _mockCardService.Setup(service => service.GetCardById(cardId)).Returns(card);

            _mockCardService.Setup(service => service.UpdateCard(It.IsAny<Card>())).Returns(true);

            var result = _controller.SetCardState(cardId, cardStateDto);

            Assert.IsType<NoContentResult>(result);


            _mockCardService.Verify(service => service.CardExists(cardId), Times.Once);
            _mockCardService.Verify(service => service.GetCardById(cardId), Times.Once);
            _mockCardService.Verify(service => service.UpdateCard(It.IsAny<Card>()), Times.Once);
        }

    }
}
