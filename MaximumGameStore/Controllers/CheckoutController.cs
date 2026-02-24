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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly MaximumGameStoreContext _context;

        public CheckoutController(MaximumGameStoreContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutDto dto)
        {
            int userId = GetUserId();

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Game)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active");

            if (cart == null || !cart.CartItems.Any())
                BadRequest("Cart is empty");

            var ownedGameIds = await _context.OrderItems
                .Where(oi => oi.Order.UserId == userId)
                .Select(oi => oi.GameId)
                .ToListAsync();

            var duplicate = cart.CartItems.Any(ci => ownedGameIds.Contains(ci.GameId));

            if (duplicate) return BadRequest("One or more games already owned");

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

                return Ok(new
                {
                    message = "Checkout successful",
                    orderId = order.Id
                });
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Checkout failed");
            }
        }

        private int GetUserId()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(Id!);
        }
    }
}
