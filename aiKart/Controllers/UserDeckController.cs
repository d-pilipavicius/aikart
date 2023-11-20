
using aiKart.Dtos.DeckDtos;
using aiKart.Dtos.UserDtos;
using aiKart.Interfaces;
using aiKart.Models;
using aiKart.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace aiKart.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserDeckController : Controller
{
    private readonly IUserDeckRepository _userDeckRepository;
    private readonly IDeckService _deckService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserDeckController(IUserDeckRepository userDeckRepository, IUserService userService, IDeckService deckService, IMapper mapper)
    {
        _mapper = mapper;
        _userDeckRepository = userDeckRepository;
        _userService = userService;
        _deckService = deckService;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserDeckDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetUserDecks()
    {
        var userDecks = _userDeckRepository.GetUserDecks();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userDeckDtos = _mapper.Map<List<UserDeckDto>>(userDecks);

        return Ok(userDeckDtos);
    }

    [HttpGet("{userId}/decks")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<DeckDto>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetDecksByUser(int userId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_userService.UserExists(userId))
            return NotFound();

        var decks = _userDeckRepository.GetDecksByUser(userId);
        var decksDto = _mapper.Map<List<DeckDto>>(decks);

        return Ok(decksDto);
    }

    [HttpGet("{deckId}/users")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<DeckDto>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetUsersOfADeck(int deckId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_deckService.DeckExistsById(deckId))
            return NotFound();

        var users = _userDeckRepository.GetUsersOfADeck(deckId);
        var usersDto = _mapper.Map<List<UserDto>>(users);

        return Ok(usersDto);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public IActionResult AddUserDeck([FromBody] UserDeckDto userDeckDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _userService.GetUser(userDeckDto.UserId);
        var deck = _deckService.GetDeckById(userDeckDto.DeckId);

        if (user == null || deck == null)
            return NotFound();

        if (_userDeckRepository.UserDeckExists(userDeckDto.UserId, userDeckDto.DeckId))
        {
            ModelState.AddModelError("", "Relationship already exists");
            return Conflict(ModelState);
        }

        var userDeck = new UserDeck
        {
            UserId = userDeckDto.UserId,
            DeckId = userDeckDto.DeckId
        };

        // Add the new relationship to the UserDeck table
        if (!_userDeckRepository.AddUserDeck(userDeck))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

}
