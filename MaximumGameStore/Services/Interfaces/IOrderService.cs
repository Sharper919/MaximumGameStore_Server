using MaximumGameStore.DTOs.Order;

namespace MaximumGameStore.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<UserOrdersDto>> GetUserOrdersAsync(int userId);
        Task<List<AllOrdersDto>> GetAllOrdersAsync();
    }
}
