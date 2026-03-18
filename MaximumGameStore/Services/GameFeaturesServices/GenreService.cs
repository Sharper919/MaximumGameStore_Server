using MaximumGameStore.Data;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;

namespace MaximumGameStore.Services.GameDetailsServices
{
    public class GenreService : IGameFeaturesService
    {
        private readonly MaximumGameStoreContext _context;

        public GenreService(MaximumGameStoreContext context)
        {
            _context = context;
        }

        public async Task<int> Create(string name)
        {
            var entity = new Genre
            {
                Name = name
            };

            _context.Genres.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> Delete(int Id)
        {
            var entity = await _context.Genres.FindAsync(Id);

            if (entity == null) return false;

            _context.Genres.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(int Id, string name)
        {
            var entity = await _context.Genres.FindAsync(Id);

            if (entity == null) return false;

            entity.Name = name;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
