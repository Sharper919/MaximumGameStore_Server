using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Services
{
    public class CartService : ICartService
    {
        private readonly MaximumGameStoreContext _context;

        public CartService(MaximumGameStoreContext context)
        {
            _context = context;
        }

        public async Task<string> AddGameAsync(int userId, int gameId)
        {
            var game = await _context.Games.FindAsync(gameId);

            if (game == null) return "Game not found";

            bool alreadyOwned = await _context.OrderItems
                .AnyAsync(oi => oi.GameId == gameId && oi.Order.UserId == userId);

            if (alreadyOwned) return "You already own this game";

            var cart = await GetOrCreateActiveCart(userId);

            bool exist = await _context.CartItems
                .AnyAsync(ci => ci.GameId == gameId && ci.CartId == cart.Id);

            if (exist) return "Game already in cart";

            CartItem cartItem = new CartItem
            {
                CartId = cart.Id,
                GameId = game.Id,
                DateTimeAdded = DateTime.UtcNow
            };

            _context.CartItems.Add(cartItem);
            cart.DateTimeUpdate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return "Game added to cart";
        }

        public async Task<string> ClearCartAsync(int userId)
        {
            var cart = await GetOrCreateActiveCart(userId);

            var items = _context.CartItems.Where(ci => ci.CartId == cart.Id);

            _context.CartItems.RemoveRange(items);
            cart.DateTimeUpdate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return "Cart cleared";
        }

        public async Task<CartDto?> GetCartAsync(int userId)
        {
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

            return new CartDto
            {
                CartId = cart.Id,
                CartItems = items,
                TotalPrice = items.Sum(i => i.Price)
            };
        }

        public async Task<string> RemoveGameAsync(int userId, int gameId)
        {
            var cart = await GetOrCreateActiveCart(userId);

            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.GameId == gameId && ci.CartId == cart.Id);

            if (item == null) return "Not found";

            _context.CartItems.Remove(item);
            cart.DateTimeUpdate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return "Game removed from cart";
        }

        private async Task<Cart> GetOrCreateActiveCart(int userId)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active");

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    DateTimeCreation = DateTime.UtcNow,
                    DateTimeUpdate = DateTime.UtcNow,
                    Status = "Active"
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }
    }
}
