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

        public async Task<int> Create(string name)
        {
            var entity = new T
            {
                Name = name
            };

            _dbSet.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> Delete(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);

            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<GameFeatureDto>> GetAll()
        {
            return await _dbSet.Select(x => new GameFeatureDto { Name = x.Name }).ToListAsync();
        }

        public async Task<bool> Update(int Id, string name)
        {
            var entity = await _dbSet.FindAsync(Id);

            if (entity == null) return false;

            entity.Name = name;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
