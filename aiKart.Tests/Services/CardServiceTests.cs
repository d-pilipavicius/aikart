using System;
using Xunit;
using Moq;
using aiKart.Services;
using aiKart.Interfaces;
using aiKart.Models;
using aiKart.States;

namespace aiKart.Tests
{
    public class CardServiceTests
    {
        private readonly Mock<ICardRepository> _mockCardRepository = new Mock<ICardRepository>();
        private readonly CardService _cardService;

        public CardServiceTests()
        {
            _cardService = new CardService(_mockCardRepository.Object);
        }

        [Fact]
        public void GetCardById_ReturnsCard_WhenCardExists()
        {
            int cardId = 1;
            Card mockCard = new Card { Id = cardId, Question = "Q", Answer = "A", State = CardState.Unanswered };
            _mockCardRepository.Setup(repo => repo.GetCardById(cardId)).Returns(mockCard);
            var result = _cardService.GetCardById(cardId);
            Assert.Equal(mockCard, result);
        }

        [Fact]
        public void CardExists_ReturnsTrue_WhenCardExists()
        {
            int cardId = 1;
            _mockCardRepository.Setup(repo => repo.CardExists(cardId)).Returns(true);
            var result = _cardService.CardExists(cardId);
            Assert.True(result);
        }

        [Fact]
        public void AddCardToDeck_ReturnsTrue_WhenAddedSuccessfully()
        {
            Card mockCard = new Card { Id = 1, Question = "Q", Answer = "A", State = CardState.Unanswered };
            _mockCardRepository.Setup(repo => repo.AddCardToDeck(mockCard)).Returns(true);
            var result = _cardService.AddCardToDeck(mockCard);
            Assert.True(result);
        }

        [Fact]
        public void DeleteCard_ReturnsFalse_WhenCardIsNull()
        {
            Card nullCard = null;
            var result = _cardService.DeleteCard(nullCard);
            Assert.False(result);
        }

        [Fact]
        public void UpdateCard_ReturnsTrue_WhenUpdatedSuccessfully()
        {
            Card mockCard = new Card { Id = 1, Question = "Q", Answer = "A", State = CardState.Unanswered };
            _mockCardRepository.Setup(repo => repo.UpdateCard(mockCard)).Returns(true);
            var result = _cardService.UpdateCard(mockCard);
            Assert.True(result);
        }

        [Fact]
        public void UpdateCardState_ReturnsTrue_WhenStateUpdatedSuccessfully()
        {
            Card mockCard = new Card { Id = 1, Question = "Q", Answer = "A", State = CardState.Unanswered };
            _mockCardRepository.Setup(repo => repo.UpdateCardState(mockCard)).Returns(true);
            var result = _cardService.UpdateCardState(mockCard);
            Assert.True(result);
        }

        [Fact]
        public void GetCardById_ReturnsNull_WhenCardNotExists()
        {
            int cardId = 1;
            _mockCardRepository.Setup(repo => repo.GetCardById(cardId)).Returns((Card)null);
            var result = _cardService.GetCardById(cardId);
            Assert.Null(result);
        }

        [Fact]
        public void CardExists_ReturnsFalse_WhenCardNotExists()
        {
            int cardId = 1;
            _mockCardRepository.Setup(repo => repo.CardExists(cardId)).Returns(false);
            var result = _cardService.CardExists(cardId);
            Assert.False(result);
        }

        [Fact]
        public void AddCardToDeck_ReturnsFalse_WhenAdditionUnsuccessful()
        {
            Card mockCard = new Card { Id = 1, Question = "Q", Answer = "A", State = CardState.Unanswered };
            _mockCardRepository.Setup(repo => repo.AddCardToDeck(mockCard)).Returns(false);
            var result = _cardService.AddCardToDeck(mockCard);
            Assert.False(result);
        }

        [Fact]
        public void DeleteCard_ReturnsFalse_WhenDeletionUnsuccessful()
        {
            Card mockCard = new Card { Id = 1, Question = "Q", Answer = "A", State = CardState.Unanswered };
            _mockCardRepository.Setup(repo => repo.DeleteCard(mockCard)).Returns(false);
            var result = _cardService.DeleteCard(mockCard);
            Assert.False(result);
        }

        [Fact]
        public void UpdateCard_ReturnsFalse_WhenUpdateUnsuccessful()
        {
            Card mockCard = new Card { Id = 1, Question = "Q", Answer = "A", State = CardState.Unanswered };
            _mockCardRepository.Setup(repo => repo.UpdateCard(mockCard)).Returns(false);
            var result = _cardService.UpdateCard(mockCard);
            Assert.False(result);
        }

        [Fact]
        public void UpdateCardState_ReturnsFalse_WhenStateUpdateUnsuccessful()
        {
            Card mockCard = new Card { Id = 1, Question = "Q", Answer = "A", State = CardState.Unanswered };
            _mockCardRepository.Setup(repo => repo.UpdateCardState(mockCard)).Returns(false);
            var result = _cardService.UpdateCardState(mockCard);
            Assert.False(result);
        }

        [Fact]
        public void UpdateCardRepetitionInterval_CardExists_UpdatesSuccessfully()
        {
            int cardId = 1;
            var initialEFactor = 2.5;
            var initialInterval = 1;
            var card = new Card
            {
                Id = cardId,
                EFactor = initialEFactor,
                IntervalInDays = initialInterval,
                LastRepetition = DateTime.Now.AddDays(-1)
            };
            _mockCardRepository.Setup(repo => repo.GetCardById(cardId)).Returns(card);
            _mockCardRepository.Setup(repo => repo.UpdateCard(card)).Returns(true);

            bool result = _cardService.UpdateCardRepetitionInterval(cardId, CardState.Answered);

            Assert.True(result);
            _mockCardRepository.Verify(repo => repo.UpdateCard(card), Times.Once);
            // Additional assertions to check EFactor and IntervalInDays
            Assert.NotEqual(initialEFactor, card.EFactor);
            Assert.NotEqual(initialInterval, card.IntervalInDays);
        }


        [Fact]
        public void UpdateCardRepetitionInterval_CardDoesNotExist_ReturnsFalse()
        {
            int cardId = 1;
            _mockCardRepository.Setup(repo => repo.GetCardById(cardId)).Returns((Card)null);

            bool result = _cardService.UpdateCardRepetitionInterval(cardId, CardState.Answered);

            Assert.False(result);
        }

        [Fact]
        public void UpdateCardRepetitionInterval_UpdateFails_ReturnsFalse()
        {
            int cardId = 1;
            var card = new Card { Id = cardId, EFactor = 2.5, IntervalInDays = 1, LastRepetition = DateTime.Now.AddDays(-1) };
            _mockCardRepository.Setup(repo => repo.GetCardById(cardId)).Returns(card);
            _mockCardRepository.Setup(repo => repo.UpdateCard(card)).Returns(false);

            bool result = _cardService.UpdateCardRepetitionInterval(cardId, CardState.Answered);

            Assert.False(result);
        }
    }
}
