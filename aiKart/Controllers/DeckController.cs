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
        private readonly IDeckRepository _deckRepository;
        private readonly IMapper _mapper;
        public DeckController(IDeckRepository deckRepository, IMapper mapper)
        {
            _deckRepository = deckRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeckDto>), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetDecks()
        {
            var decks = _deckRepository.GetDecks();

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
            if (!_deckRepository.DeckExistsById(deckId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deck = _deckRepository.GetDeck(deckId);

            var deckDto = _mapper.Map<DeckDto>(deck);

            return Ok(deckDto);
        }


        // return card dtos
        [HttpGet("cardlist/{deckId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CardDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCardsInDeck(int deckId)
        {
            if (!_deckRepository.DeckExistsById(deckId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cards = _deckRepository.GetDeckCards(deckId);
            var cardDtos = _mapper.Map<IEnumerable<CardDto>>(cards);

            return Ok(cardDtos);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult AddDeck([FromBody] AddDeckDto deckDto)
        {
            if (deckDto == null)
                return BadRequest(ModelState);

            if (_deckRepository.DeckExistsByName(deckDto.Name))
                return UnprocessableEntity("Deck with name: " + deckDto.Name + " already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deck = _mapper.Map<Deck>(deckDto);

            if (!_deckRepository.AddDeck(deck))
            {
                ModelState.AddModelError("", "Something went wrong while saving a new deck");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPut("{deckId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateDeck(int deckId, [FromBody] UpdateDeckDto deckDto) //change addDeckDto 
        {
            if (deckDto == null)
                return BadRequest(ModelState);

            if (!_deckRepository.DeckExistsById(deckId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deck = _deckRepository.GetDeck(deckId);

            _mapper.Map(deckDto, deck);

            if (!_deckRepository.UpdateDeck(deck))
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
            if (!_deckRepository.DeckExistsById(deckId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deckToDelete = _deckRepository.GetDeck(deckId);

            if (!_deckRepository.DeleteDeck(deckToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the card");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }

}