using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly MaximumGameStoreContext _context;
        public GamesController(MaximumGameStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetGames([FromQuery] string? search, [FromQuery] int? genreId, [FromQuery] int? developerId, [FromQuery] int? engineId)
        {
            var query = _context.Games
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .Include(g => g.GameDevelopers)
                    .ThenInclude(gd => gd.Developer)
                .Include(g => g.GameEngines)
                    .ThenInclude(ge => ge.Engine)
                .Include(g => g.GameImages)
                .AsQueryable();

            // Пошук за назвою
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(g => g.Name.Contains(search));
            }

            // Фільтри
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

            var games = await query
                .Select(g => new GameListItemDto
                {
                    Id = g.Id,
                    Title = g.Name,
                    Price = g.Price,
                    MainImage = g.GameImages
                        .Where(i => i.IsMain)
                        .Select(i => i.ImagePath)
                        .FirstOrDefault(),
                    Genres = g.GameGenres
                        .Select(gg => gg.Genre.Name)
                        .ToList()
                })
                .ToListAsync();

            return Ok(games);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            var game = await _context.Games
            .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
            .Include(g => g.GameDevelopers)
                .ThenInclude(gd => gd.Developer)
            .Include(g => g.GameEngines)
                .ThenInclude(ge => ge.Engine)
            .Include(g => g.GamePublishers)
                .ThenInclude(gp => gp.Publisher)
            .Include(g => g.GameModes)
                .ThenInclude(ge => ge.Mode)
            .Include(g => g.GameImages)
            .Include (g => g.Series)
            .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
                return NotFound();

            var dto = new GameDetailsDto
            {
                Id = game.Id,
                Title = game.Name,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Serie = game.Series?.Name,
                Genres = game.GameGenres.Select(g => g.Genre.Name).ToList(),
                Developers = game.GameDevelopers.Select(d => d.Developer.Name).ToList(),
                Engines = game.GameEngines.Select(e => e.Engine.Name).ToList(),
                Publishers = game.GamePublishers.Select(p => p.Publisher.Name).ToList(),
                Modes = game.GameModes.Select(m => m.Mode.Name).ToList(),
                Images = game.GameImages
                    .OrderBy(i => i.SortOrder)
                    .Select(i => i.ImagePath)
                    .ToList()
            };

            return Ok(dto);

        }

    }
}
