using MaximumGameStore.DTOs;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MaximumGameStore.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        private int GetUserId()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(Id!);
        }

        [HttpGet("home")]
        public async Task<IActionResult> GetUserInfo()
        {
            int userId = GetUserId();

            var userInfo = await _userService.GetUserInfoAsync(userId);

            return Ok(userInfo);
        }

        [HttpGet("games")]
        public async Task<IActionResult> GetUserGames()
        {
            int userId = GetUserId();

            var userGames = await _userService.GetUserGamesAsync(userId);

            return Ok(userGames);
        }

        [HttpPut("update/username")]
        public async Task<IActionResult> UpdateUserName(UpdateUserNameDto dto)
        {
            int userId = GetUserId();

            var result = await _userService.UpdateUserNameAsync(userId, dto);

            if (result.statusCode == 404) return NotFound(result.massage);

            if (result.statusCode == 400) return BadRequest(result.massage);

            return Ok(new { message = result.massage });
        }

        [HttpPut("update/password")]
        public async Task<IActionResult> UpdateUserPassword(UpdateUserPasswordDto dto)
        {
            int userId = GetUserId();

            var result = await _userService.UpdateUserPasswordAsync(userId, dto);

            if (result.statusCode == 404) return NotFound(result.massage);

            if (result.statusCode == 400) return BadRequest(result.massage);

            return Ok(new { message = result.massage });
        }

        [HttpPut("delete")]
        public async Task<IActionResult> DeleteUser()
        {
            int userId = GetUserId();

            var result = await _userService.DeleteUserAsync(userId);

            if (result.statusCode == 404) return NotFound(result.massage);

            return Ok(new { message = result.massage });
        }
    }
}
