using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MaximumGameStore.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly MaximumGameStoreContext _context;

        public CartController(MaximumGameStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int userId = GetUserId();

            var cart = await GetOrCreateActiveCart(userId);

            var items = await _context.CartItems.Where(ci => ci.CartId == cart.Id)
                .Select(ci => new CartItemDto
                {
                    GameId = ci.GameId,
                    Title = ci.Game.Name,
                    Price = ci.Game.Price,
                    Image = ci.Game.GameImages.Where(gi => gi.IsMain)
                            .Select(gi => gi.ImagePath)
                            .FirstOrDefault(),

                }).ToListAsync();

            return Ok(new CartDto
            {
                CartId = cart.Id,
                CartItems = items,
                TotalPrice = items.Sum(i => i.Price)
            });
        }

        [HttpPost("add/{gameId:int}")]
        public async Task<IActionResult> AddGameToCart(int gameId)
        {
            int userId = GetUserId();

            var game = await _context.Games.FindAsync(gameId);

            if (game == null) return NotFound("Game not found");

            bool alreadyOwned = await _context.OrderItems
                .AnyAsync(oi => oi.GameId == gameId && oi.Order.UserId == userId);

            if (alreadyOwned) return BadRequest("You already own this game");

            var cart = await GetOrCreateActiveCart(userId);

            bool exist = await _context.CartItems
                .AnyAsync(ci => ci.GameId == gameId && ci.CartId == cart.Id);

            if (exist) return BadRequest("Game already in cart");

            CartItem cartItem = new CartItem
            {
                CartId = cart.Id,
                GameId = game.Id,
                DateTimeAdded = DateTime.UtcNow
            };

            _context.CartItems.Add(cartItem);
            cart.DateTimeUpdate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Game added to cart" });
        }

        [HttpDelete("remove/{gameId:int}")]
        public async Task<IActionResult> RemoveGameFromCart(int gameId)
        {
            int userId = GetUserId();

            var cart = await GetOrCreateActiveCart(userId);

            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.GameId == gameId && ci.CartId == cart.Id);

            if (item == null) return NotFound();

            _context.CartItems.Remove(item);
            cart.DateTimeUpdate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Game removed from cart" });
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            int userId = GetUserId();

            var cart = await GetOrCreateActiveCart(userId);

            var items = _context.CartItems.Where(ci => ci.CartId == cart.Id);

            _context.CartItems.RemoveRange(items);
            cart.DateTimeUpdate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cart cleared" });
        }

        private async Task<Cart> GetOrCreateActiveCart(int userId)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active");

            if(cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    DateTimeCreation = DateTime.UtcNow,
                    DateTimeUpdate = DateTime.UtcNow,
                    Status = "Active"
                };
            }

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        private int GetUserId()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(Id!);
        }
    }
}
