using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;

namespace MaximumGameStore.Services
{
    public class SystemRequirementService : ISystemRequirementService
    {
        private readonly MaximumGameStoreContext _context;

        public SystemRequirementService(MaximumGameStoreContext context)
        {
            _context = context;
        }

        public async Task<int> AddRequirementsAsync(int gameId, AddRequirementsDto dto)
        {
            var requirement = new SystemRequirement
            {
                GameId = dto.GameId,
                RequirementType = dto.RequirementType,
                Os = dto.Os,
                Cpu = dto.Cpu,
                Gpu = dto.Gpu,
                RamGb = dto.RamGb,
                StorageGb = dto.StorageGb,
                DirectX = dto.DirectX
            };

            _context.SystemRequirements.Add(requirement);
            await _context.SaveChangesAsync();

            return requirement.Id;
        }
    }
}
