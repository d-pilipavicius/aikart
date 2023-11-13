using System.Numerics;
using aiKart.Dtos.CardDtos;
using aiKart.Dtos.DeckDtos;
using aiKart.Dtos.UserDtos;
using aiKart.Interfaces;
using aiKart.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace aiKart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckController : Controller
    {
        private readonly IDeckService _deckService;
        private readonly IUserDeckRepository _userDeckRepository;
        private readonly IMapper _mapper;

        public DeckController(IDeckService deckService, IUserDeckRepository userDeckRepository, IMapper mapper)
        {
            _deckService = deckService;
            _userDeckRepository = userDeckRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeckDto>), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetAllDecks()
        {
            var decks = _deckService.GetAllDecksIncludingCards();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deckDtos = _mapper.Map<List<DeckDto>>(decks);

            return Ok(deckDtos);
        }

        [HttpGet("{deckId}")]
        [ProducesResponseType(200, Type = typeof(DeckDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetDeck(int deckId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_deckService.DeckExistsById(deckId))
                return NotFound();

            var deck = _deckService.GetDeckById(deckId);

            var deckDto = _mapper.Map<DeckDto>(deck);

            return Ok(deckDto);
        }


        [HttpGet("cardlist/{deckId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CardDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCardsInDeck(int deckId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_deckService.DeckExistsById(deckId))
                return NotFound();

            var cards = _deckService.GetCardsInDeck(deckId);
            var cardDtos = _mapper.Map<IEnumerable<CardDto>>(cards);

            return Ok(cardDtos);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(int))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult AddDeck([FromBody] AddDeckDto deckDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (deckDto == null)
                return BadRequest(ModelState);

            if (_deckService.DeckExistsByName(deckDto.Name))
                return UnprocessableEntity("Deck with name: " + deckDto.Name + " already exists");

            var deck = _mapper.Map<Deck>(deckDto);

            if (!_deckService.AddDeck(deck))
            {
                ModelState.AddModelError("", "Something went wrong while saving a new deck");
                return StatusCode(500, ModelState);
            }

            _userDeckRepository.AddUserDeck(new UserDeck { UserId = deckDto.CreatorId, DeckId = deck.Id });

            return CreatedAtAction(nameof(GetDeck), new { deckId = deck.Id }, deck.Id);
        }

        [HttpPut("{deckId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateDeck(int deckId, [FromBody] UpdateDeckDto deckDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (deckDto == null)
                return BadRequest(ModelState);

            if (!_deckService.DeckExistsById(deckId))
                return NotFound();

            var deck = _deckService.GetDeckById(deckId);

            if (deck == null)
                return NotFound();

            _mapper.Map(deckDto, deck);

            if (!_deckService.UpdateDeck(deck))
            {
                ModelState.AddModelError("", "Something went wrong updating deck");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{deckId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult DeleteDeck(int deckId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_deckService.DeckExistsById(deckId))
                return NotFound();

            var deckToDelete = _deckService.GetDeckById(deckId);

            if (deckToDelete == null)
                return NotFound();

            if (!_deckService.DeleteDeck(deckToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the card");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
