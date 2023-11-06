using aiKart.Dtos.DeckDtos;
using aiKart.Dtos.UserDtos;
using aiKart.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace aiKart.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetUsers()
    {
        var users = _userService.GetUsers();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userDtos = _mapper.Map<List<UserDto>>(users);

        return Ok(userDtos);
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetUser(int userId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_userService.UserExists(userId))
            return NotFound();

        var user = _userService.GetUser(userId);
        var userDto = _mapper.Map<UserDto>(user);

        return Ok(userDto);
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

        var decks = _userService.GetDecksByUser(userId);
        var decksDto = _mapper.Map<List<DeckDto>>(decks);

        return Ok(decksDto);
    }
}
