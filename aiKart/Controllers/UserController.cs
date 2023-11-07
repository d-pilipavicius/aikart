
using aiKart.Dtos.UserDtos;
using aiKart.Interfaces;
using aiKart.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace aiKart.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IDeckService _deckService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IDeckService deckService, IMapper mapper)
    {
        _mapper = mapper;
        _userService = userService;
        _deckService = deckService;
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

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(int))]
    [ProducesResponseType(400)]
    public IActionResult AddUser([FromBody] UserDto userCreate)
    {
        if (userCreate == null)
            return BadRequest();

        var existingUser = _userService.GetUsers()
            .FirstOrDefault(u => u.Name.Trim().ToUpper() == userCreate.Name.TrimEnd().ToUpper());

        if (existingUser != null)
        {
            var existingUserDto = _mapper.Map<UserResponseDto>(existingUser);
            return Ok(existingUserDto);
        }

        var userMap = _mapper.Map<User>(userCreate);

        if (!_userService.AddUser(userMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        var userDto = _mapper.Map<UserResponseDto>(userMap);

        return CreatedAtAction(nameof(GetUser), new { userId = userMap.Id }, userDto);
    }
}
