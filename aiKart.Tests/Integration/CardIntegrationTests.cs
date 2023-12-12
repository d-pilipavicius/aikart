using Xunit;
using aiKart.Data;
using aiKart.Models;
using aiKart.Repositories;
using aiKart.Services;
using aiKart.Dtos.CardDtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace aiKart.Tests.Integration
{
    public class CardIntegrationTests : IDisposable
    {
        private readonly CardService _cardService;
        private readonly DataContext _context;
        private readonly IMapper _mapper; 

        public CardIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Cards_" + Guid.NewGuid())
                .Options;

            _context = new DataContext(options);
            var cardRepository = new CardRepository(_context);

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Card, CardDto>();
                cfg.CreateMap<CardDto, Card>();
            }).CreateMapper();

            _cardService = new CardService(cardRepository);
        }

        [Fact]
        public async Task AddCardToDeck_CardIsValid_ShouldAddCard()
        {
            // Arrange
            var newCard = new Card { Question = "Test Question", Answer = "Test Answer", DeckId = 1 };

            // Act
            var result = _cardService.AddCardToDeck(newCard);

            // Assert
            Assert.True(result);
            var cardInDb = await _context.Cards.FirstOrDefaultAsync(c => c.Question == "Test Question");
            Assert.NotNull(cardInDb);
        }

        [Fact]
        public void GetCardById_CardExists_ShouldReturnCard()
        {
            // Arrange
            var existingCard = new Card { Question = "Existing Question", Answer = "Existing Answer" };
            _context.Cards.Add(existingCard);
            _context.SaveChanges();

            // Act
            var card = _cardService.GetCardById(existingCard.Id);

            // Assert
            Assert.NotNull(card);
            Assert.Equal("Existing Question", card.Question);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
