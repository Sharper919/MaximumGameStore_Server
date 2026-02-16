using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Models;
using MaximumGameStore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MaximumGameStoreContext _context;
        private readonly PasswordHasher _hasher;
        private readonly JwtService _jwt;

        public AuthController(MaximumGameStoreContext context, PasswordHasher hasher, JwtService jwt)
        {
            _context = context;
            _hasher = hasher;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("This email already exists");

            if (await _context.Users.AnyAsync(u => u.Name == dto.UserName))
                return BadRequest("This user name already exists");

            var user = new User
            {
                Email = dto.Email,
                Name = dto.UserName,
                PasswordHash = _hasher.Hash(dto.Password),
                DateTimeRegistration = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwt.CreateToken(user);

            var response = new AuthResponseDto
            {
                Token = token,
                UserName = user.Name
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == dto.UserName);

            if (user == null)
                return Unauthorized();

            if (!_hasher.Verify(dto.Password, user.PasswordHash))
                return Unauthorized();

            var token = _jwt.CreateToken(user);

            var response = new AuthResponseDto
            {
                Token = token,
                UserName = user.Name
            };

            return Ok(response);
        }
    }
}
