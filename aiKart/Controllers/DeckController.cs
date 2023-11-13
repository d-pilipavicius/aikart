using aiKart.Dtos.CardDtos;
using aiKart.Dtos.DeckDtos;
using aiKart.Exceptions;
using aiKart.Interfaces;
using aiKart.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions; // For NullLogger

namespace aiKart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckController : Controller
    {
        private readonly IDeckService _deckService;
        private readonly IUserDeckService _userDeckService;
        private readonly IMapper _mapper;
        private readonly ILogger<DeckController> _logger;

        // Primary constructor used by the application
        public DeckController(IDeckService deckService, IUserDeckService userDeckService, IMapper mapper, ILogger<DeckController> logger)
        {
            _deckService = deckService;
            _userDeckService = userDeckService;
            _mapper = mapper;
            _logger = logger;

            // Attach event handlers to the DeckService events
            _deckService.DeckCreated += DeckCreatedHandler;
            _deckService.DeckUpdated += DeckUpdatedHandler;

        }

        // Additional constructor for backward compatibility and testing
        public DeckController(IDeckService deckService, IUserDeckService userDeckService, IMapper mapper)
            : this(deckService, userDeckService, mapper, NullLogger<DeckController>.Instance)
        {
        }

        private void DeckCreatedHandler(Deck deck)
        {
            // Log or handle the deck creation logic
            _logger.LogInformation($"Deck created with ID: {deck.Id}");
        }

        private void DeckUpdatedHandler(Deck deck)
        {
            // Log or handle the deck update logic
            _logger.LogInformation($"Deck updated with ID: {deck.Id}");
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
                _logger.LogError("Deck data was not provided");
                return BadRequest("Deck data must be provided.");
            }

            var deck = _mapper.Map<Deck>(deckDto);

            // Here's where you can put some validation logic before saving the deck
            if (string.IsNullOrWhiteSpace(deck.Name))
            {
                throw new EntityValidationException<Deck>(deck, "Deck name must not be empty.");
            }

            if (_deckService.DeckExistsByName(deck.Name))
            {
                throw new EntityValidationException<Deck>(deck, $"Deck with name: {deck.Name} already exists");
            }

            _deckService.AddDeck(deck);
            var userDeck = new UserDeck { UserId = deckDto.CreatorId, DeckId = deck.Id };
            _userDeckService.AddUserDeck(userDeck);

            return CreatedAtAction(nameof(GetDeck), new { deckId = deck.Id }, _mapper.Map<DeckDto>(deck));
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
