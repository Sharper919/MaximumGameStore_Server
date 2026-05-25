using MaximumGameStore.DTOs;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaximumGameStore.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            var response = await _authService.AddUserAsync(dto);

            if (response == null)
                return BadRequest("This email or user name already exists");

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            var response = await _authService.AuthenticateAsync(dto);

            if (response == null) return BadRequest("Invalid credentials");

            return Ok(response);
        }
    }
}
