using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MaximumGameStore.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly MaximumGameStoreContext _context;

        public UserController(MaximumGameStoreContext context)
        {
            _context = context;
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

            var userInfo = await _context.Users.Where(u => u.Id == userId)
                .Select(ui => new UserInfoDto
                {
                    Id = ui.Id,
                    UserName = ui.Name,
                    Email = ui.Email,
                    CreatedAt = ui.DateTimeRegistration
                }).FirstOrDefaultAsync();

            if(userInfo == null)
                return NotFound();

            return Ok(userInfo);
        }

        [HttpGet("user-games")]
        public async Task<IActionResult> GetUserGames()
        {
            int userId = GetUserId();

            var userGames = await _context.OrderItems.Where(oi => oi.Order.UserId == userId)
                .Select(oi => new UserGamesDto
                {
                    Id = oi.Id,
                    Title = oi.Game.Name,
                    Price = oi.PriceAtPurchase,
                    MainImage = oi.Game.GameImages.Where(gi => gi.IsMain)
                        .Select(gi => gi.ImagePath).FirstOrDefault(),
                    PurchasedAt = oi.Order.DateTimeOrder
                }).Distinct().ToListAsync();

            if(userGames == null) return NotFound();

            return Ok(userGames);
        }

        [HttpPut("update/username")]
        public async Task<IActionResult> UpdateUserName(UpdateUserNameDto dto)
        {
            int userId = GetUserId();
            string newUserName = dto.NewUserName.Trim();

            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.IsDeleted) return NotFound();

            if (string.IsNullOrWhiteSpace(newUserName))
                return BadRequest("Username is empty");

            bool exists = await _context.Users
                .AnyAsync(u => u.Name == newUserName && u.Id != userId && !u.IsDeleted);

            if (exists) return BadRequest("Username already taken");

            user.Name = newUserName;
            await _context.SaveChangesAsync();

            return Ok(new { message = "UserName Updated" });
        }

        [HttpPut("update/password")]
        public async Task<IActionResult> UpdateUserPassword(UpdateUserPasswordDto dto)
        {
            int userId = GetUserId();
            PasswordHasher hasher = new PasswordHasher();

            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.IsDeleted) return NotFound();

            if (!hasher.Verify(dto.OldPassword, user.PasswordHash))
                return BadRequest(new { message = "Old password is incorrect" });

            if (hasher.Verify(dto.NewPassword, user.PasswordHash))
                return BadRequest(new { message = "This is the old password" });

            user.PasswordHash = hasher.Hash(dto.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password Updated" });
        }

        [HttpPut("delete")]
        public async Task<IActionResult> DeleteUser()
        {
            int userId = GetUserId();

            var user = await _context.Users.FindAsync(userId);

            if(user == null || user.IsDeleted) return NotFound();

            user.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Account deactivated" });
        }
    }
}
