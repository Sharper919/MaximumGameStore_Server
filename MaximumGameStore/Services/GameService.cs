using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Services
{
    public class GameService : IGameService
    {
        private readonly MaximumGameStoreContext _context;

        public GameService(MaximumGameStoreContext context)
        {
            _context = context;
        }

        public async Task<List<GameListItemDto>> FilterAsync(int? genreId, 
            int? developerId, int? engineId, 
            int? serieId, int? publisherId, int? modeId)
        {
            var query = _context.Games.AsNoTracking().AsQueryable();

            if (genreId.HasValue)
            {
                query = query.Where(g =>
                    g.GameGenres.Any(gg => gg.GenreId == genreId));
            }

            if (developerId.HasValue)
            {
                query = query.Where(g =>
                    g.GameDevelopers.Any(gd => gd.DeveloperId == developerId));
            }

            if (engineId.HasValue)
            {
                query = query.Where(g =>
                    g.GameEngines.Any(ge => ge.EngineId == engineId));
            }

            if (serieId.HasValue)
            {
                query = query.Where(g => g.SeriesId == serieId);
            }

            if (publisherId.HasValue)
            {
                query = query.Where(g =>
                    g.GamePublishers.Any(gp => gp.PublisherId == publisherId));
            }

            if (modeId.HasValue)
            {
                query = query.Where(g =>
                    g.GameModes.Any(gp => gp.ModeId == modeId));
            }

            return await query
                .Select(g => new GameListItemDto
                {
                    Id = g.Id,
                    Title = g.Name,
                    Price = g.Price,
                    MainImage = g.GameImages
                        .Where(i => i.IsMain)
                        .Select(i => i.ImagePath)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task<GameDetailsDto?> GetGameDetailsByIdAsync(int gameId)
        {
            var game = await _context.Games.AsNoTracking().Where(g => g.Id == gameId)
                .Select(g => new GameDetailsDto
                {
                    Id = g.Id,
                    Title = g.Name,
                    Price = g.Price,
                    ReleaseDate = g.ReleaseDate,
                    Serie = g.Series != null ? g.Series.Name : null,
                    Description = g.Description,
                    Genres = g.GameGenres.Select(gg => gg.Genre.Name).ToList(),
                    Developers = g.GameDevelopers.Select(gd => gd.Developer.Name).ToList(),
                    Engines = g.GameEngines.Select(ge => ge.Engine.Name).ToList(),
                    Publishers = g.GamePublishers.Select(gp => gp.Publisher.Name).ToList(),
                    Modes = g.GameModes.Select(gm => gm.Mode.Name).ToList()
                })
                .FirstOrDefaultAsync();

            if (game == null) return null;

            return game;
        }

        public async Task<List<string>> GetGameImagesByIdAsync(int gameId)
        {
            return await _context.GameImages.AsNoTracking().Where(gi => gi.GameId == gameId)
                .OrderBy(gi => gi.SortOrder)
                .Select(gi => gi.ImagePath)
                .ToListAsync();
        }

        public async Task<List<GameListItemDto>> GetGamesAsync(int take = 8)
        {
            return await _context.Games.AsNoTracking()
                .OrderByDescending(g => g.ReleaseDate)
                .Take(take)
                .Select(g => new GameListItemDto
                {
                    Id = g.Id,
                    Title = g.Name,
                    Price = g.Price,
                    MainImage = g.GameImages
                                .Where(i => i.IsMain)
                                .Select(i => i.ImagePath)
                                .FirstOrDefault(),
                }).ToListAsync();
        }

        public async Task<List<GameListItemDto>> GetGamesByNameAsync(string name)
        {
            var query = _context.Games.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(g => g.Name.Contains(name));
            }

            return await query.Select(g => new GameListItemDto
            {
                Id = g.Id,
                Title = g.Name,
                Price = g.Price,
                MainImage = g.GameImages
                        .Where(i => i.IsMain)
                        .Select(i => i.ImagePath)
                        .FirstOrDefault()
            }).ToListAsync();
        }

        public async Task<List<GameSystemRequirementsDto>> GetGamesRequirementsByIdAsync(int gameId)
        {
            return await _context.SystemRequirements.AsNoTracking().Where(sr => sr.GameId == gameId)
                .Select(sr => new GameSystemRequirementsDto
                {
                    Id = sr.Id,
                    RequirementType = sr.RequirementType,
                    Os = sr.Os,
                    Cpu = sr.Cpu,
                    Gpu = sr.Gpu,
                    RamGb = sr.RamGb,
                    StorageGb = sr.StorageGb,
                    DirectX = sr.DirectX
                })
                .ToListAsync();
        }
    }
}
