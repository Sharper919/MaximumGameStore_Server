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

            if (result.statusCode == 404) return NotFound(result.massage);
            if (result.statusCode == 400) return BadRequest(result.massage);

            return Ok(new { message = result.massage });
        }

        [HttpDelete("remove/{gameId:int}")]
        public async Task<IActionResult> RemoveGameFromCart(int gameId)
        {
            int userId = GetUserId();

            var result = await _cartService.RemoveGameAsync(userId, gameId);

            if (result.statusCode == 404) return NotFound(result.massage);

            return Ok(new { message = result.massage });
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
