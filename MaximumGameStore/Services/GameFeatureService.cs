using MaximumGameStore.Data;
using MaximumGameStore.DTOs.GameDetails;
using MaximumGameStore.Models.Interfaces;
using MaximumGameStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Services
{
    public class GameFeatureService<T> : IGameFeatureService<T> where T : class, IGameFeatureEntity, new()
    {
        private readonly MaximumGameStoreContext _context;
        private readonly DbSet<T> _dbSet;

        public GameFeatureService(MaximumGameStoreContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<int> CreateAsync(string name)
        {
            name = name.Trim();

            var existing = await _dbSet
                .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

            if (existing != null)
            {
                return existing.Id;
            }

            var entity = new T
            {
                Name = name
            };

            _dbSet.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<List<GameFeatureDto>> GetAllAsync()
        {
            return await _dbSet
                .OrderBy(x => x.Name)
                .Select(x => new GameFeatureDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }
    }
}
