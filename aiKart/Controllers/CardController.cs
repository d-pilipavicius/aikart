using System;
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_cardService.CardExists(cardId))
                return NotFound();

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (cardStateDto == null)
                return BadRequest(ModelState);

            if (!_cardService.CardExists(cardId))
                return NotFound();

            var card = _cardService.GetCardById(cardId);

            try
            {
                _mapper.Map(cardStateDto, card);
            }
            catch (Exception)
            {
                return BadRequest("Invalid state!");
            }

            if (card == null)
                return NotFound();

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (cardDto == null)
                return BadRequest(ModelState);

            if (!_deckService.DeckExistsById(cardDto.DeckId))
                return NotFound("Deck with id " + cardDto.DeckId + " does not exist");

            var card = _mapper.Map<Card>(cardDto);

            if (!_cardService.AddCardToDeck(card))
            {
                ModelState.AddModelError("", "Something went wrong while saving a new card");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPut("{cardId}")]
        public IActionResult UpdateCard(int cardId, [FromBody] UpdateCardDto cardDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_cardService.CardExists(cardId))
                return NotFound();

            var card = _cardService.GetCardById(cardId);
            if (card == null)
                return NotFound();

            try
            {
                _mapper.Map(cardDto, card);
                var updateResult = _cardService.UpdateCard(card);
                if (!updateResult)
                {
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
            catch (Exception) // Catch exceptions from the service layer.
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

            return NoContent();
        }


        [HttpDelete("{cardId}")]
        public IActionResult DeleteCard(int cardId)
        {
            if (!_cardService.CardExists(cardId))
                return NotFound();

            var cardToDelete = _cardService.GetCardById(cardId);
            if (cardToDelete == null) // This check might be redundant if CardExists already ensures the card is there
                return NotFound();

            var result = _cardService.DeleteCard(cardToDelete);
            if (!result)
                return StatusCode(500, "An error occurred while attempting to delete the card.");

            return NoContent();
        }


        [HttpPut("update-repetition-interval")]
        public IActionResult UpdateCardRepetitionInterval([FromQuery] int cardId, [FromQuery] string stateValue)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CardState state;
            if (!Enum.TryParse<CardState>(stateValue, out state))
            {
                return StatusCode(400, "State value is invalid");
            }
            Console.WriteLine(state);

            if (!_cardService.CardExists(cardId))
                return NotFound();

            var card = _cardService.GetCardById(cardId);
            if (card == null)
                return NotFound();

            bool updateSuccess = _cardService.UpdateCardRepetitionInterval(cardId, state);

            if (updateSuccess)
                return NoContent();
            else
                return StatusCode(500);
        }


    }

}