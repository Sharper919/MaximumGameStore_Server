using MaximumGameStore.DTOs.GameDetails;
using MaximumGameStore.Models.Interfaces;

namespace MaximumGameStore.Services.Interfaces
{
    public interface IGameFeatureService<T> where T : class, IGameFeatureEntity
    {
        Task<List<GameFeatureDto>> GetAllAsync();
        Task<int> CreateAsync(string name);
    }
}
