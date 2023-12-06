using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using aiKart.Controllers;
using aiKart.Dtos.TriviaDtos;
using aiKart.Dtos.DeckDtos;
using aiKart.Interfaces;
using aiKart.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace aiKart.Tests.Controllers
{
    public class TriviaControllerTests
    {

        [Fact]
        public async Task GetTriviaDeck_InvalidCategory_ReturnsBadRequest()
        {
            // Arrange
            var deckServiceMock = new Mock<IDeckService>();
            var userDeckServiceMock = new Mock<IUserDeckService>();
            var cardServiceMock = new Mock<ICardService>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var mapperMock = new Mock<IMapper>();

            var controller = new TriviaController(
                deckServiceMock.Object,
                userDeckServiceMock.Object,
                cardServiceMock.Object,
                httpClientFactoryMock.Object,
                mapperMock.Object);

            var requestDto = new TriviaDeckRequestDto(CategoryId: 0, NumberOfCards: 10, CreatorId: 1); // Invalid category

            // Act
            var result = await controller.GetTriviaDeck(requestDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid category.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetTriviaDeck_InvalidNumberOfCards_ReturnsBadRequest()
        {
            // Arrange
            var deckServiceMock = new Mock<IDeckService>();
            var userDeckServiceMock = new Mock<IUserDeckService>();
            var cardServiceMock = new Mock<ICardService>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var mapperMock = new Mock<IMapper>();

            var controller = new TriviaController(
                deckServiceMock.Object,
                userDeckServiceMock.Object,
                cardServiceMock.Object,
                httpClientFactoryMock.Object,
                mapperMock.Object);

            var requestDto = new TriviaDeckRequestDto(CategoryId: 9, NumberOfCards: 0, CreatorId: 1); // Invalid number of cards

            // Act
            var result = await controller.GetTriviaDeck(requestDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid number of cards.", badRequestResult.Value);
        }


    }
}
