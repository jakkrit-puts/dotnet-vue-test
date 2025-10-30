using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Models;
using UserAPI.Services;

namespace UserAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly ApplicationDbContext _context;
    private readonly JwtService _jwtService;

    public UserController(ApplicationDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username) ||
           string.IsNullOrWhiteSpace(user.Password))
        {
            return BadRequest("All fields are required");
        }

        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        {
            return BadRequest("Username already exists");
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var responseUser = new
        {
            user.Id,
            user.Username,
        };

        return CreatedAtAction(nameof(Register), new { id = user.Id }, responseUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<dynamic>> Login([FromBody] User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
        {
            return BadRequest("Username and password are required");
        }

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == user.Username);

        if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
        {
            return Unauthorized("Invalid username or password");
        }

        var token = _jwtService.GenerateToken(existingUser);

        return Ok(new
        {
            token = token,
        });
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid token.");
        }

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(new
        {
            user = new
            {
                id = user.Id,
                username = user.Username
            }
        });
    }
}