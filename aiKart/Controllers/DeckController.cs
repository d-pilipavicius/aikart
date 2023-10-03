using aiKart.Interfaces;
using aiKart.Models;
using Microsoft.AspNetCore.Mvc;

namespace aiKart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckController : ControllerBase
    {
        private readonly IDeckRepository _deckRepository;
        public DeckController(IDeckRepository deckRepository) 
        {
            _deckRepository = deckRepository;
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
        [HttpGet("{}")]
        [ProducesResponseType(200, Type = typeof(Deck))]
        public IActionResult GetDeck(int deckId)
        {
            var deck = _deckRepository.GetDeck(deckId);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(deck);
        }
        [HttpPut("{deckId}")]
        public IActionResult UpdateDeck(int deckId, Deck updatedDeck)
        {
            return Ok();
        }

    }

}