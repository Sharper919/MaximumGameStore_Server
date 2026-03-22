using MaximumGameStore.DTOs.GameDetails;
using MaximumGameStore.Models.Interfaces;

namespace MaximumGameStore.Services.Interfaces
{
    public interface IGameFeatureService<T> where T : class, IGameFeatureEntity
    {
        Task<List<GameFeatureDto>> GetAll();
        Task<int> Create(string name);
        Task<bool> Update(int Id, string name);
        Task<bool> Delete(int Id);
    }
}
