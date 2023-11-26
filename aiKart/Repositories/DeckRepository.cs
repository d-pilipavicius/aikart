using aiKart.Data;
using aiKart.Interfaces;
using aiKart.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using aiKart.States;


namespace aiKart.Repositories
{
    public class DeckRepository : IDeckRepository
    {
        private readonly DataContext _context;

        public DeckRepository(DataContext context)
        {
            _context = context;
        }

        //  async/await
        public async Task<Deck> GetDeckByIdAsync(int deckId)
        {
            return await _context.Decks.FindAsync(deckId);
        }

        public IEnumerable<Deck> GetDecksIncludingCards()
        {
            return _context.Decks.Include(d => d.Cards).ToList();
        }

        public ICollection<Deck> GetDecks()
        {
            return _context.Decks.OrderBy(d => d.Id).ToList();
        }

        public Deck? GetDeck(int id)
        {
            return _context.Decks.Find(id);
        }

        public ICollection<Card> GetDeckCards(int deckId)
        {
            return _context.Cards
                .Where(c => c.DeckId == deckId).ToList();
        }

        public bool DeckExistsById(int id)
        {
            return _context.Decks.Any(d => d.Id == id);
        }

        public bool DeckExistsByName(string name)
        {
            return _context.Decks.Any(d => d.Name == name);
        }

        public bool AddDeck(Deck deck)
        {
            _context.Decks.Add(deck);
            return Save();
        }

        public bool DeleteDeck(Deck deck)
        {
            _context.Decks.Remove(deck);
            return Save();
        }

        public bool UpdateDeck(Deck deck)
        {
            _context.Decks.Update(deck);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public async Task<Deck> CloneDeckAsync(Deck originalDeck)
        {
            var clonedDeck = new Deck
            {
                Name = originalDeck.Name,
                Description = originalDeck.Description,
                CreatorName = originalDeck.CreatorName,
                IsPublic = false,
                CreationDate = originalDeck.CreationDate,
                LastEditDate = DateTime.UtcNow,
                Cards = originalDeck.Cards.Select(c => new Card
                {
                    Question = c.Question,
                    Answer = c.Answer,
                    State = CardState.Unanswered,
                }).ToList()
            };

            _context.Decks.Add(clonedDeck);
            await _context.SaveChangesAsync();

            return clonedDeck;
        }
    }
}
