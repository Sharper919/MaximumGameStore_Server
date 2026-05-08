using MaximumGameStore.DTOs.Images;
using MaximumGameStore.Models;

namespace MaximumGameStore.Services.Interfaces
{
    public interface IGameImageService
    {
        Task<List<GameImage>?> UploadManyAsync(UploadGameImagesDto dto);
    }
}
