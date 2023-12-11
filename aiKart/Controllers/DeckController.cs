using aiKart.Dtos.CardDtos;
using aiKart.Dtos.DeckDtos;
using aiKart.Dtos.UserDtos;
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

        public DeckController(
            IDeckService deckService,
            IUserDeckService userDeckService,
            IMapper mapper,
            ILogger<DeckController> logger = null) // Logger is optional
        {
            _deckService = deckService ?? throw new ArgumentNullException(nameof(deckService));
            _userDeckService = userDeckService ?? throw new ArgumentNullException(nameof(userDeckService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<DeckController>(); // Provide a default logger if none is supplied

            _deckService.DeckCreated += DeckCreatedHandler;
            _deckService.DeckUpdated += DeckUpdatedHandler;

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

        [HttpGet("public")]
        public IActionResult GetAllPublicDecks()
        {
            var publicDecks = _deckService.GetAllDecksIncludingCards().Where(d => d.IsPublic);
            var deckDtos = _mapper.Map<List<DeckDto>>(publicDecks);
            return Ok(deckDtos);
        }


        [HttpGet("{deckId}")]
        public async Task<IActionResult> GetDeck(int deckId)
        {
            if (!_deckService.DeckExistsById(deckId))
            {
                return NotFound();
            }

            var deck = await _deckService.GetDeckByIdAsync(deckId);
            if (deck == null)
            {
                return NotFound();
            }

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

        [HttpGet("cardlist/user/{deckId}")]
        public IActionResult GetUserCardsInDeck(int deckId)
        {
            if (!_deckService.DeckExistsById(deckId))
            {
                return NotFound();
            }
            var cards = _deckService.GetUserCardsInDeck(deckId);
            var cardDtos = _mapper.Map<IEnumerable<UserCardDto>>(cards);
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

        [HttpGet("cardlist/shuffled/{deckId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CardDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetShuffledDeckCards(int deckId)
        {
            if (!_deckService.DeckExistsById(deckId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cards = _deckService.GetShuffledDeckCards(deckId);
            var cardDtos = _mapper.Map<IEnumerable<CardDto>>(cards);

            return Ok(cardDtos);
        }

        [HttpPost("{deckId}/clone")]
        public async Task<IActionResult> ClonePublicDeck(int deckId, [FromBody] CloneDeckDto cloneDeckDto)
        {
            try
            {
                var clonedDeck = await _deckService.ClonePublicDeck(deckId, cloneDeckDto.UserId);
                var deckDto = _mapper.Map<DeckDto>(clonedDeck);
                return CreatedAtAction(nameof(GetDeck), new { deckId = clonedDeck.Id }, deckDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("set-anki-usage")]
        public IActionResult SetAnkiUsage([FromQuery] int deckId, [FromQuery] bool useAnki)
        {
            if (!_deckService.DeckExistsById(deckId))
            {
                return NotFound();
            }

            if (!_deckService.SetAnkiUsage(deckId, useAnki))
            {
                ModelState.AddModelError("", "Something went wrong while updating anki usage in deck");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpPut("reset-repetition-interval/{deckId}")]
        public IActionResult ResetRepetitionIntervalForDeckCards(int deckId)
        {
            if (!_deckService.DeckExistsById(deckId))
            {
                return NotFound();
            }

            if (!_deckService.ResetRepetitionIntervalForDeckCards(deckId))
            {
                ModelState.AddModelError("", "Something went wrong while updating interval for deck cards");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
