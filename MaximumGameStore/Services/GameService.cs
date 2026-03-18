using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Models;
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

        // user functions
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

        // admin functions
        public async Task<int?> CreateGameAsync(CreateGameDto dto)
        {
            if (_context.Games.Any(g => g.Name == dto.Name))
            {
                return null;
            }

            var game = new Game
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ReleaseDate = dto.ReleaseDate,
                SeriesId = dto.SeriesId
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            int gameId = game.Id;

            await AddRelations(gameId, dto);

            return gameId;
        }

        public async Task<bool> UpdateGameAsync(int gameId, CreateGameDto dto)
        {
            var game = await _context.Games.FindAsync(gameId);

            if (game == null) return false;

            game.Name = dto.Name;
            game.Description = dto.Description;
            game.Price = dto.Price;
            game.ReleaseDate = dto.ReleaseDate;
            game.SeriesId = dto.SeriesId;

            await RemoveRelations(gameId);
            await AddRelations(gameId, dto);

            return true;
        }

        public async Task<bool> DeleteGameAsync(int gameId)
        {
            var game = await _context.Games.FindAsync(gameId);

            if (game == null) return false;

            game.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task AddRelations(int gameId, CreateGameDto dto)
        {
            foreach (var id in dto.GenreIds)
            {
                _context.GameGenres.Add(new GameGenre
                {
                    GameId = gameId,
                    GenreId = id
                });
            }

            foreach (var id in dto.DeveloperIds)
            {
                _context.GameDevelopers.Add(new GameDeveloper
                {
                    GameId = gameId,
                    DeveloperId = id
                });
            }

            foreach (var id in dto.PublisherIds)
            {
                _context.GamePublishers.Add(new GamePublisher
                {
                    GameId = gameId,
                    PublisherId = id
                });
            }

            foreach (var id in dto.ModeIds)
            {
                _context.GameModes.Add(new GameMode
                {
                    GameId = gameId,
                    ModeId = id
                });
            }

            foreach (var id in dto.EngineIds)
            {
                _context.GameEngines.Add(new GameEngine
                {
                    GameId = gameId,
                    EngineId = id
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task RemoveRelations(int gameId)
        {
            var genres = _context.GameGenres.Where(x => x.GameId == gameId);
            var developers = _context.GameDevelopers.Where(x => x.GameId == gameId);
            var publishers = _context.GamePublishers.Where(x => x.GameId == gameId);
            var modes = _context.GameModes.Where(x => x.GameId == gameId);
            var engines = _context.GameEngines.Where(x => x.GameId == gameId);

            _context.GameGenres.RemoveRange(genres);
            _context.GameDevelopers.RemoveRange(developers);
            _context.GamePublishers.RemoveRange(publishers);
            _context.GameModes.RemoveRange(modes);
            _context.GameEngines.RemoveRange(engines);

            await _context.SaveChangesAsync();
        }
    }
}
