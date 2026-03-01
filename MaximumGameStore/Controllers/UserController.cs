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

        [HttpGet("user-games")]
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

            string result = await _userService.UpdateUserNameAsync(userId, dto);

            if (result == "Not found") return NotFound();

            if (result == "Username is empty")
                return BadRequest(result);

            if (result == "Username already taken") return BadRequest(result);

            return Ok(new { message = result });
        }

        [HttpPut("update/password")]
        public async Task<IActionResult> UpdateUserPassword(UpdateUserPasswordDto dto)
        {
            int userId = GetUserId();

            var result = await _userService.UpdateUserPasswordAsync(userId, dto);

            if (result == "Not found") return NotFound();

            if (result == "Old password is incorrect")
                return BadRequest(result);

            if (result == "This is the old password")
                return BadRequest(result);

            return Ok(new { message = result });
        }

        [HttpPut("delete")]
        public async Task<IActionResult> DeleteUser()
        {
            int userId = GetUserId();

            string result = await _userService.DeleteUserAsync(userId);

            if (result == "Not found") return NotFound();

            return Ok(new { message = result });
        }
    }
}
