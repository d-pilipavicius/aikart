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
        private readonly ICardRepository _cardRepository;
        private readonly IDeckRepository _deckRepository;
        private readonly IMapper _mapper;
        public CardController(ICardRepository cardRepository, IDeckRepository deckRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _deckRepository = deckRepository;
            _mapper = mapper;
        }

        [HttpGet("{cardId}")]
        [ProducesResponseType(200, Type = typeof(CardDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCard(int cardId)
        {
            if (!_cardRepository.CardExists(cardId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var card = _cardRepository.GetCard(cardId);

            var cardDto = _mapper.Map<CardDto>(card);

            return Ok(cardDto);
        }

        [HttpGet("states")]
        [ProducesResponseType(200, Type = typeof(List<String>))]
        public IActionResult GetStateTypes()
        {
            var cardStateValues = Enum.GetValues(typeof(CardState))
                .Cast<CardState>()
                .Select(e => e.ToString())
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

            if (!_cardRepository.CardExists(cardId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var card = _cardRepository.GetCard(cardId);

            try
            {
                _mapper.Map(cardStateDto, card);
            }
            catch (Exception e)
            {
                return BadRequest("Invalid state!");
            }

            if (!_cardRepository.UpdateCard(card))
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

            if (!_deckRepository.DeckExistsById(cardDto.DeckId))
                return NotFound("Deck with id " + cardDto.DeckId + " does not exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var card = _mapper.Map<Card>(cardDto);

            if (!_cardRepository.AddCardToDeck(card))
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

            if (!_cardRepository.CardExists(cardId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var card = _cardRepository.GetCard(cardId);

            _mapper.Map(cardDto, card);

            if (!_cardRepository.UpdateCard(card))
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
            if (!_cardRepository.CardExists(cardId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cardToDelete = _cardRepository.GetCard(cardId);

            if (!_cardRepository.DeleteCard(cardToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the card");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }

}