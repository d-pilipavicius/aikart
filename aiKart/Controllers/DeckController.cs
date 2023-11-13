using aiKart.Dtos.CardDtos;
using aiKart.Dtos.DeckDtos;
using aiKart.Interfaces;
using aiKart.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace aiKart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckController : Controller
    {
        private readonly IDeckService _deckService;
        private readonly IUserDeckService _userDeckService;
        private readonly IMapper _mapper;

        public DeckController(IDeckService deckService, IUserDeckService userDeckService, IMapper mapper)
        {
            _deckService = deckService;
            _userDeckService = userDeckService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllDecks()
        {
            var decks = _deckService.GetAllDecksIncludingCards();
            var deckDtos = _mapper.Map<List<DeckDto>>(decks);
            return Ok(deckDtos);
        }

        [HttpGet("{deckId}")]
        public IActionResult GetDeck(int deckId)
        {
            if (!_deckService.DeckExistsById(deckId))
            {
                return NotFound();
            }

            var deck = _deckService.GetDeckById(deckId);
            var deckDto = _mapper.Map<DeckDto>(deck);
            return Ok(deckDto);
        }

        [HttpGet("cardlist/{deckId}")]
        public IActionResult GetCardsInDeck(int deckId)
        {
            if (!_deckService.DeckExistsById(deckId))
            {
                return NotFound();
            }

            var cards = _deckService.GetCardsInDeck(deckId);
            var cardDtos = _mapper.Map<IEnumerable<CardDto>>(cards);
            return Ok(cardDtos);
        }

        [HttpPost]
        public IActionResult AddDeck([FromBody] AddDeckDto deckDto)
        {
            if (deckDto == null)
            {
                return BadRequest("Deck data must be provided.");
            }

            if (_deckService.DeckExistsByName(deckDto.Name))
            {
                return UnprocessableEntity($"Deck with name: {deckDto.Name} already exists");
            }

            var deck = _mapper.Map<Deck>(deckDto);

            if (!_deckService.AddDeck(deck))
            {
                ModelState.AddModelError("", "Something went wrong while saving a new deck");
                return StatusCode(500, ModelState);
            }

            var userDeck = new UserDeck { UserId = deckDto.CreatorId, DeckId = deck.Id };
            if (!_userDeckService.AddUserDeck(userDeck))
            {
                ModelState.AddModelError("", "Something went wrong while adding the deck to the user");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction(nameof(GetDeck), new { deckId = deck.Id }, deck.Id);
        }

        [HttpPut("{deckId}")]
        public IActionResult UpdateDeck(int deckId, [FromBody] UpdateDeckDto deckDto)
        {
            if (deckDto == null)
            {
                return BadRequest("Update data must be provided.");
            }

            if (!_deckService.DeckExistsById(deckId))
            {
                return NotFound();
            }

            var deck = _deckService.GetDeckById(deckId);
            if (deck == null)
            {
                return NotFound();
            }

            _mapper.Map(deckDto, deck);

            if (!_deckService.UpdateDeck(deck))
            {
                ModelState.AddModelError("", "Something went wrong updating deck");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{deckId}")]
        public IActionResult DeleteDeck(int deckId)
        {
            if (!_deckService.DeckExistsById(deckId))
            {
                return NotFound();
            }

            var deckToDelete = _deckService.GetDeckById(deckId);
            if (deckToDelete == null)
            {
                return NotFound();
            }

            if (!_deckService.DeleteDeck(deckToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the deck");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
