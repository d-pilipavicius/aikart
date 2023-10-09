using aiKart.Dtos.CardDtos;
using aiKart.Interfaces;
using aiKart.Models;
using aiKart.States;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace aiKart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IDeckService _deckService;
        private readonly IMapper _mapper;
        public CardController(ICardService cardService, IDeckService deckService, IMapper mapper)
        {
            _cardService = cardService;
            _deckService = deckService;
            _mapper = mapper;
        }

        [HttpGet("{cardId}")]
        [ProducesResponseType(200, Type = typeof(CardDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCard(int cardId)
        {
            if (!_cardService.CardExists(cardId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var card = _cardService.GetCardById(cardId);

            var cardDto = _mapper.Map<CardDto>(card);

            return Ok(cardDto);
        }

        [HttpGet("states")]
        [ProducesResponseType(200, Type = typeof(List<CardState>))]
        public IActionResult GetStateTypes()
        {
            var cardStateValues = Enum.GetValues(typeof(CardState))
                .Cast<CardState>()
                .ToList();

            return Ok(cardStateValues);
        }

        [HttpPut("states/set/{cardId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult SetCardState(int cardId, [FromBody] CardStateDto cardStateDto)
        {
            if (cardStateDto == null)
                return BadRequest(ModelState);

            if (!_cardService.CardExists(cardId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var card = _cardService.GetCardById(cardId);

            try
            {
                _mapper.Map(cardStateDto, card);
            }
            catch (Exception e)
            {
                return BadRequest("Invalid state!");
            }

            if (!_cardService.UpdateCard(card))
            {
                ModelState.AddModelError("", "Something went wrong while updating a card");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult AddCard([FromBody] AddCardDto cardDto)
        {
            if (cardDto == null)
                return BadRequest(ModelState);

            if (!_deckService.DeckExistsById(cardDto.DeckId))
                return NotFound("Deck with id " + cardDto.DeckId + " does not exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var card = _mapper.Map<Card>(cardDto);

            if (!_cardService.AddCardToDeck(card))
            {
                ModelState.AddModelError("", "Something went wrong while saving a new card");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPut("{cardId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateCard(int cardId, [FromBody] UpdateCardDto cardDto)
        {
            if (cardDto == null)
                return BadRequest(ModelState);

            if (!_cardService.CardExists(cardId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var card = _cardService.GetCardById(cardId);

            _mapper.Map(cardDto, card);

            if (!_cardService.UpdateCard(card))
            {
                ModelState.AddModelError("", "Something went wrong while updating a card");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{cardId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCard(int cardId)
        {
            if (!_cardService.CardExists(cardId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cardToDelete = _cardService.GetCardById(cardId);

            if (!_cardService.DeleteCard(cardToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the card");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }

}