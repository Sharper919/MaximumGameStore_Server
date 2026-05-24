using MaximumGameStore.DTOs;

namespace MaximumGameStore.Services.Interfaces
{
    public interface ISystemRequirementService
    {
            Task<int> AddRequirementsAsync(int gameId, AddRequirementsDto dto);
    }
}
