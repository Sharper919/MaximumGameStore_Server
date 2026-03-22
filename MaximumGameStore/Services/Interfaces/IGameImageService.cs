using MaximumGameStore.DTOs.Images;
using MaximumGameStore.Models;

namespace MaximumGameStore.Services.Interfaces
{
    public interface IGameImageService
    {
        Task<List<GameImage>> GetByGameId(int gameId);
        Task<GameImage?> Upload(UploadGameImageDto dto);
        Task<bool> Delete(int imageId);
        Task<bool> SetMain(int imageId);

    }
}
