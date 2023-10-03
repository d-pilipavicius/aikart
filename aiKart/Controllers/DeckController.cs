using aiKart.Interfaces;
using aiKart.Models;
using Microsoft.AspNetCore.Mvc;

namespace aiKart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckController : Controller
    {
        private readonly IDeckRepository _deckRepository;
        private readonly ICardRepository _cardRepository;
        public DeckController(IDeckRepository deckRepository, ICardRepository cardRepository) 
        {
            _deckRepository = deckRepository;
            _cardRepository = cardRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<Deck>), 200)]
        public IActionResult GetDecks()
        {
            var decks = _deckRepository.GetDecks();
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(decks);
        }
        [HttpGet("{deckId}")]
        [ProducesResponseType(200, Type = typeof(Deck))]
        [ProducesResponseType(400)]
        public IActionResult GetDeck(int deckId)
        {
            if(_deckRepository.GetDeck(deckId) == null)
                return NotFound();
                
            var deck = _deckRepository.GetDeck(deckId);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(deck);
        }
        [HttpDelete("{deckId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteDeck(int deckId)
        {
            if(_deckRepository.GetDeck(deckId) == null)
                return NotFound();
            
            var deckToDelete = _deckRepository.GetDeck(deckId);
            var cardsToDelete = _deckRepository.GetDeckCards(deckId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            foreach(Card card in cardsToDelete)
            {
                if(!_cardRepository.DeleteCard(card))
                {
                    ModelState.AddModelError("", "Something went wrong while deleting a card");
                }
            }

            if(!_deckRepository.DeleteDeck(deckToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the deck");
            }

            return NoContent();
        }
    }

}