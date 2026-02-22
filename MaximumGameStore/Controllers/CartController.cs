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
