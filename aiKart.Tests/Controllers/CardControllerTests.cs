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
        public void GetCard_InvalidModelState_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Error", "Model State is Invalid");
            var result = _controller.GetCard(1);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddCard_InvalidModelState_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Error", "Model State is Invalid");
            var addCardDto = new AddCardDto(1, "Question", "Answer");
            var result = _controller.AddCard(addCardDto);
            Assert.IsType<BadRequestObjectResult>(result);
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
            var card = new Card { Id = cardId, Question = "Question", Answer = "Answer" }; // Populate with necessary properties.

            // Set up the mock to return true for CardExists check
            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);
            // Ensure GetCardById returns a card object for the provided ID
            _mockCardService.Setup(service => service.GetCardById(cardId)).Returns(card);
            // Ensure UpdateCard returns true for the card update operation
            _mockCardService.Setup(service => service.UpdateCard(card)).Returns(true);

            // Act
            var result = _controller.UpdateCard(cardId, updateCardDto);

            // Assert
            Assert.IsType<NoContentResult>(result); // Verify that NoContentResult is returned by the action

            // Verify the service calls
            _mockCardService.Verify(service => service.CardExists(cardId), Times.Once); // Verify CardExists is called once
            _mockCardService.Verify(service => service.GetCardById(cardId), Times.Once); // Verify GetCardById is called once with the correct ID
            _mockCardService.Verify(service => service.UpdateCard(card), Times.Once); // Verify UpdateCard is called once with the correct card object
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

        [Fact]
        public void AddCard_DeckDoesNotExist_ReturnsNotFound()
        {
            var addCardDto = new AddCardDto(1, "Question", "Answer");
            _mockDeckService.Setup(service => service.DeckExistsById(addCardDto.DeckId)).Returns(false);
            var result = _controller.AddCard(addCardDto);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateCard_UpdateFails_ReturnsStatusCode500()
        {
            var cardId = 1;
            var updateCardDto = new UpdateCardDto("Updated Question", "Updated Answer");
            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);
            _mockCardService.Setup(service => service.GetCardById(cardId)).Returns(new Card());
            _mockCardService.Setup(service => service.UpdateCard(It.IsAny<Card>())).Returns(false); // Setup to return false, indicating failure.

            var result = _controller.UpdateCard(cardId, updateCardDto);

            var objectResult = Assert.IsType<ObjectResult>(result); // Assert that you receive an ObjectResult
            Assert.Equal(500, objectResult.StatusCode); // Assert that the status code is 500
        }


        [Fact]
        public void DeleteCard_DeleteFails_ReturnsStatusCode500()
        {
            var cardId = 1;
            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);
            _mockCardService.Setup(service => service.GetCardById(cardId)).Returns(new Card());
            _mockCardService.Setup(service => service.DeleteCard(It.IsAny<Card>())).Returns(false);

            var result = _controller.DeleteCard(cardId);

            Assert.IsType<ObjectResult>(result); // We expect an ObjectResult for a server error
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode); // Status code should be 500 for server error
        }


        [Fact]
        public void SetCardState_InvalidModelState_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Error", "Model State is Invalid");
            var cardStateDto = new CardStateDto(CardState.Answered);
            var result = _controller.SetCardState(1, cardStateDto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void SetCardState_InvalidStateDto_ReturnsBadRequest()
        {
            var cardId = 1;
            CardStateDto cardStateDto = null; // Invalid DTO

            var result = _controller.SetCardState(cardId, cardStateDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddCard_NullDto_ReturnsBadRequest()
        {
            AddCardDto cardDto = null; // Null DTO

            var result = _controller.AddCard(cardDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddCard_FailToAddCard_ReturnsStatusCode500()
        {
            var cardDto = new AddCardDto(1, "Question", "Answer");
            _mockDeckService.Setup(service => service.DeckExistsById(cardDto.DeckId)).Returns(true);
            _mockCardService.Setup(service => service.AddCardToDeck(It.IsAny<Card>())).Returns(false);

            var result = _controller.AddCard(cardDto);

            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public void UpdateCard_NullDto_ReturnsNotFound()
        {
            var cardId = 1;
            UpdateCardDto cardDto = null; // Null DTO

            var result = _controller.UpdateCard(cardId, cardDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateCard_InvalidModelState_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Error", "Model State is Invalid");
            var updateCardDto = new UpdateCardDto("Updated Question", "Updated Answer");

            var result = _controller.UpdateCard(1, updateCardDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateCardRepetitionInterval_ValidCard_ReturnsNoContent()
        {
            var cardId = 1;

            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);
            _mockCardService.Setup(service => service.GetCardById(cardId)).Returns(new Card());
            _mockCardService.Setup(service => service.UpdateCardRepetitionInterval(cardId, CardState.Answered)).Returns(true);

            var result = _controller.UpdateCardRepetitionInterval(cardId, "Answered");

            Assert.IsType<NoContentResult>(result);
        }



        [Fact]
        public void UpdateCardRepetitionInterval_CardDoesNotExist_ReturnsNotFound()
        {
            var cardId = 1;
            var state = "Answered";

            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(false);

            var result = _controller.UpdateCardRepetitionInterval(cardId, state);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateCardRepetitionInterval_UpdateFails_ReturnsStatusCode500()
        {
            var cardId = 1;
            var state = "Answered";

            _mockCardService.Setup(service => service.CardExists(cardId)).Returns(true);
            _mockCardService.Setup(service => service.GetCardById(cardId)).Returns(new Card());
            _mockCardService.Setup(service => service.UpdateCardRepetitionInterval(cardId, CardState.Answered)).Returns(false);

            var result = _controller.UpdateCardRepetitionInterval(cardId, state);

            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = result as StatusCodeResult;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }


        [Fact]
        public void UpdateCardRepetitionInterval_InvalidModelState_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Error", "Model State is Invalid");
            var result = _controller.UpdateCardRepetitionInterval(1, "Answered");

            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}
