using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly MaximumGameStoreContext _context;
        public CheckoutService(MaximumGameStoreContext context)
        {
            _context = context;
        }

        public async Task<(CheckoutResponseDto? dto, int statusCode, string massage)> CheckoutAsync(int userId, CheckoutDto dto)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Game)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active");

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                return (null, 400, "Cart is empty");
                    
            var ownedGameIds = await _context.OrderItems
                .Where(oi => oi.Order.UserId == userId)
                .Select(oi => oi.GameId)
                .ToListAsync();

            var duplicate = cart.CartItems.Any(ci => ownedGameIds.Contains(ci.GameId));

            if (duplicate) return (null, 400, "One or more games already owned");

            decimal total = cart.CartItems.Sum(ci => ci.Game.Price);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    UserId = userId,
                    DateTimeOrder = DateTime.UtcNow,
                    TotalAmount = total,
                    Status = "Paid"
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                var orderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    OrderId = order.Id,
                    GameId = ci.GameId,
                    PriceAtPurchase = ci.Game.Price
                });

                _context.OrderItems.AddRange(orderItems);

                var payment = new CardPayment
                {
                    OrderId = order.Id,
                    DateTimePayment = DateTime.UtcNow,
                    Amount = total,
                    CardLast4 = dto.CardNumber[^4..],
                    PaymentStatus = "Success",
                    CardType = dto.CardType
                };

                _context.CardPayments.Add(payment);

                cart.Status = "Ordered";
                cart.DateTimeUpdate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return (new CheckoutResponseDto
                {
                    OrderId = order.Id,
                    ResponseMassage = "Checkout successful"
                }, 200, "Checkout successful");
            }
            catch
            {
                await transaction.RollbackAsync();
                return (null, 500, "Checkout failed"); //StatusCode(500, "Checkout failed");
            }
        }

        public async Task<(CheckoutResponseDto? dto, int statusCode, string massage)> BuyNowAsync(int userId, int gameId, CheckoutDto dto)
        {
            var owned = await _context.OrderItems
                .AnyAsync(oi => oi.GameId == gameId && oi.Order.UserId == userId && oi.Order.Status == "Paid");

            if (owned) return (null, 400, "You already own this game");

            var game = await _context.Games.FindAsync(gameId);

            if (game == null) return (null, 404, "Game not found");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    UserId = userId,
                    DateTimeOrder = DateTime.UtcNow,
                    TotalAmount = game.Price,
                    Status = "Paid"
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                var item = new OrderItem
                {
                    OrderId = order.Id,
                    GameId = gameId,
                    PriceAtPurchase = game.Price
                };

                _context.OrderItems.Add(item);

                var payment = new CardPayment
                {
                    OrderId = order.Id,
                    CardLast4 = dto.CardNumber[^4..],
                    CardType = dto.CardType,
                    DateTimePayment = DateTime.UtcNow,
                    Amount = game.Price,
                    PaymentStatus = "Success"
                };

                _context.CardPayments.Add(payment);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return (new CheckoutResponseDto
                {
                    ResponseMassage = "Game purchased successfully",
                    OrderId = order.Id
                }, 200, "Game purchased successfully");
            }
            catch
            {
                await transaction.RollbackAsync();
                return (null, 500, "Checkout failed");
            }
        }
    }
}
