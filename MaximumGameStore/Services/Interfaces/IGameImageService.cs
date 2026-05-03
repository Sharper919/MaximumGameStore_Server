using MaximumGameStore.DTOs.Images;
using MaximumGameStore.Models;

namespace MaximumGameStore.Services.Interfaces
{
    public interface IGameImageService
    {
        Task<List<GameImage>> GetByGameIdAsync(int gameId);
        Task<GameImage?> UploadAsync(UploadGameImageDto dto);
        Task<bool> SetMainAsync(int imageId);
    }
}
