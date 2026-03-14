using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> DeleteRequirementAsync(int requirementId)
        {
            var requirement = await _context.SystemRequirements.FindAsync(requirementId);

            if (requirement == null) return false;

            _context.SystemRequirements.Remove(requirement);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateRequirementAsync(int requirementId, UpdateRequirementDto dto)
        {
            var requirement = await _context.SystemRequirements.FindAsync(requirementId);

            if (requirement == null) return false;

            requirement.RequirementType = dto.RequirementType;
            requirement.Os = dto.Os;
            requirement.Cpu = dto.Cpu;
            requirement.Gpu = dto.Gpu;
            requirement.RamGb = dto.RamGb;
            requirement.StorageGb = dto.StorageGb;
            requirement.DirectX = dto.DirectX;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
