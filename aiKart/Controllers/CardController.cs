using aiKart.Interfaces;
using aiKart.Models;
using Microsoft.AspNetCore.Mvc;

namespace aiKart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : Controller
    {
        private readonly ICardRepository _cardRepository;
        public CardController(ICardRepository cardRepository) 
        {
            _cardRepository = cardRepository;
        }
        [HttpGet("{}")]
        [ProducesResponseType(200, Type = typeof(Deck))]
        public IActionResult GetCard(int cardId)
        {
            var card = _cardRepository.GetCard(cardId);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(card);
        }

        [HttpDelete("{}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCard(int cardId)
        {
            if(!_cardRepository.CardExists(cardId))
                return NotFound();
            
            var cardToDelete = _cardRepository.GetCard(cardId);

            if(!_cardRepository.DeleteCard(cardToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the card");
            }

            return NoContent();
        }
    }

}