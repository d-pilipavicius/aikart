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
            // Arrange
            var cardId = 1;
            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);
            _mockCardService.Setup(service => service.GetCardById(cardId)).Returns(new Card());
            _mockMapper.Setup(mapper => mapper.Map<CardDto>(It.IsAny<Card>())).Returns(new CardDto(1, 1, "Question", "Answer", "Active"));

            // Act
            var result = _controller.GetCard(cardId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public void GetCard_CardDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var cardId = 1;
            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(false);

            // Act
            var result = _controller.GetCard(cardId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetStateTypes_ReturnsOk()
        {
            // Arrange
            // Act
            var result = _controller.GetStateTypes();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteCard_ValidCard_ReturnsNoContent()
        {
            // Arrange
            var cardId = 1;
            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);
            _mockCardService.Setup(service => service.GetCardById(cardId)).Returns(new Card());

            // Act
            var result = _controller.DeleteCard(cardId);

            // Diagnostic print
            Console.WriteLine($"DeleteCard: Actual result type: {result.GetType()}");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void AddCard_ValidCard_ReturnsOk()
        {
            // Arrange
            var addCardDto = new AddCardDto(1, "Question", "Answer");
            _mockDeckService.Setup(service => service.DeckExistsById(addCardDto.DeckId)).Returns(true);

            // Act
            var result = _controller.AddCard(addCardDto);

            // Diagnostic print
            Console.WriteLine($"AddCard: Actual result type: {result.GetType()}");

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateCard_ValidCard_ReturnsNoContent()
        {
            // Arrange
            var cardId = 1;
            var updateCardDto = new UpdateCardDto("Updated Question", "Updated Answer");
            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);

            // Act
            var result = _controller.UpdateCard(cardId, updateCardDto);

            // Diagnostic print
            Console.WriteLine($"UpdateCard: Actual result type: {result.GetType()}");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void SetCardState_ValidCardState_ReturnsNoContent()
        {
            // Arrange
            var cardId = 1;
            var cardStateDto = new CardStateDto("Active"); // Using string to match your DTO
            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);
            _mockCardService.Setup(service => service.GetCardById(cardId)).Returns(new Card());

            // Act
            var result = _controller.SetCardState(cardId, cardStateDto);

            // Diagnostic print
            Console.WriteLine($"SetCardState: Actual result type: {result.GetType()}");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}