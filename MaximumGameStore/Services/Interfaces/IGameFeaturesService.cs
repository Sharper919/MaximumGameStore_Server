namespace MaximumGameStore.Services.Interfaces
{
    public interface IGameFeaturesService
    {
        Task<int> Create(string name);
        Task<bool> Update(int Id, string name);
        Task<bool> Delete(int Id);
    }
}
