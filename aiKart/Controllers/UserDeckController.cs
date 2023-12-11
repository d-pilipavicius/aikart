using System.Net.Mail;
using aiKart.Dtos.DeckDtos;
using aiKart.Dtos.UserDtos;
using aiKart.Interfaces;
using aiKart.Migrations;
using aiKart.Models;
using aiKart.States;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace aiKart.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserDeckController : Controller
{
    private readonly IUserDeckService _userDeckService;
    private readonly IMapper _mapper;

    public UserDeckController(IUserDeckService userDeckService, IMapper mapper)
    {
        _mapper = mapper;
        _userDeckService = userDeckService;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserDeckDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetUserDecks()
    {
        var userDecks = _userDeckService.GetUserDecks();
        var userDeckDtos = _mapper.Map<List<UserDeckDto>>(userDecks);
        return Ok(userDeckDtos);
    }

    [HttpGet("{userId}/decks")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<DeckDto>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetDecksByUser(int userId)
    {
        var decks = _userDeckService.GetDecksByUser(userId);
        if (decks == null || decks.Count == 0)
            return NotFound();

        var decksDto = _mapper.Map<List<DeckDto>>(decks);
        return Ok(decksDto);
    }

    [HttpGet("{deckId}/users")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetUsersOfADeck(int deckId)
    {
        var users = _userDeckService.GetUsersOfADeck(deckId);
        if (users == null || users.Count == 0)
            return NotFound();

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
        if (userDeckDto == null)
            return BadRequest("UserDeck data must be provided.");

        if (_userDeckService.UserDeckExists(userDeckDto.UserId, userDeckDto.DeckId))
            return Conflict("Relationship already exists");

        var userDeck = _mapper.Map<UserDeck>(userDeckDto);
        if (!_userDeckService.AddUserDeck(userDeck))
            return StatusCode(500, "Something went wrong while saving");

        return NoContent();
    }

    [HttpDelete("{userId}/decks/{deckId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteUserDeck(int userId, int deckId)
    {
        if (!_userDeckService.UserDeckExists(userId, deckId))
            return NotFound();

        if (!_userDeckService.DeleteUserDeck(userId, deckId))
            return StatusCode(500, "Something went wrong while deleting the user deck");

        return NoContent();
    }

    [HttpPut("increment-answer-count/{userId}/{deckId}")]
    public IActionResult UpdateAnswerCount(int userId, int deckId, [FromQuery] string stateValue, [FromQuery] int count)
    {

        if (!_userDeckService.UserDeckExists(userId, deckId))
            return NotFound();

        CardState state;

        if (!Enum.TryParse<CardState>(stateValue, out state))
        {
            return StatusCode(400, "State value is invalid");
        }

        if (count <= 0)
        {
            return StatusCode(400, "Invalid count value, must be more than 0");
        }

        if (!_userDeckService.UpdateAnswerCount(userId, deckId, state, count))
        {
            return StatusCode(500, "Failed to update user deck answer count");
        };
        return NoContent();
    }

    [HttpGet("user-deck-statistics/{userId}/{deckId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserDeckStatisticsDto>))]
    public IActionResult GetUserDeckStatistics(int userId, int deckId)
    {

        if (!_userDeckService.UserDeckExists(userId, deckId))
            return NotFound();

        UserDeck userDeck = _userDeckService.GetUserDeckStatisctics(userId, deckId);
        var dto = _mapper.Map<UserDeckStatisticsDto>(userDeck);

        return Ok(dto);
    }

    [HttpPut("increment-deck-solves/{userId}/{deckId}")]
    public IActionResult IncrementDeckSolves(int userId, int deckId, [FromQuery] int count)
    {

        if (!_userDeckService.UserDeckExists(userId, deckId))
            return NotFound();

        if (count <= 0)
        {
            return StatusCode(400, "Invalid count value, must be more than 0");
        }

        if (!_userDeckService.IncrementDeckSolves(userId, deckId, count))
        {
            return StatusCode(500, "Failed to update user deck solves");
        };

        return NoContent();
    }
}
