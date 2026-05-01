using MaximumGameStore.Data;
using MaximumGameStore.DTOs.Order;
using MaximumGameStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly MaximumGameStoreContext _context;

        public OrderService(MaximumGameStoreContext context)
        {
            _context = context;
        }

        public async Task<List<AllOrdersDto>> GetAllOrdersAsync()
        {
            return await _context.Orders.Select(o => new AllOrdersDto
            {
                OrderId = o.Id,
                UserName = o.User.Name,
                Date = o.DateTimeOrder,
                Price = o.TotalAmount,
                Status = o.Status
            }).ToListAsync();
        }

        public async Task<List<UserOrdersDto>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new UserOrdersDto
                {
                    Date = o.DateTimeOrder,
                    Price = o.TotalAmount,
                    Status = o.Status
                }).ToListAsync();
        }
    }
}
