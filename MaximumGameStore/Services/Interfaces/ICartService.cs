using MaximumGameStore.DTOs;

namespace MaximumGameStore.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto?> GetCartAsync(int userId);
        Task<string> AddGameAsync(int userId, int gameId);
        Task<string> RemoveGameAsync(int userId, int gameId);
        Task<string> ClearCartAsync(int userId);
    }
}
