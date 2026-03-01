using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MaximumGameStore.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int userId = GetUserId();
            return Ok(await _cartService.GetCartAsync(userId));
        }

        [HttpPost("add/{gameId:int}")]
        public async Task<IActionResult> AddGameToCart(int gameId)
        {
            int userId = GetUserId();

            var result = await _cartService.AddGameAsync(userId, gameId);

            if (result == "Game not found") return NotFound();
            if (result == "You already own this game") return BadRequest(result);
            if (result == "Game already in cart") return BadRequest(result);

            return Ok(new { message = result });
        }

        [HttpDelete("remove/{gameId:int}")]
        public async Task<IActionResult> RemoveGameFromCart(int gameId)
        {
            int userId = GetUserId();

            var result = await _cartService.RemoveGameAsync(userId, gameId);

            if (result == "Not found") return NotFound();

            return Ok(new { message = result });
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            int userId = GetUserId();

            var result = await _cartService.ClearCartAsync(userId);

            return Ok(new { message = result });
        }

        private int GetUserId()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(Id!);
        }
    }
}
