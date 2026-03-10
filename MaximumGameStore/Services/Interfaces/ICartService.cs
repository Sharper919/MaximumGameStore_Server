using MaximumGameStore.DTOs;

namespace MaximumGameStore.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto?> GetCartAsync(int userId);
        Task<(string massage, int statusCode)> AddGameAsync(int userId, int gameId);
        Task<(string massage, int statusCode)> RemoveGameAsync(int userId, int gameId);
        Task<string> ClearCartAsync(int userId);
    }
}
