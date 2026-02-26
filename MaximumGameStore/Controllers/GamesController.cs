using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<IActionResult> GetGames()
        {
            var games = await _context.Games.OrderByDescending(g => g.ReleaseDate)
                .Take(8)
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

            return Ok(games);
        }

        [HttpGet("filter-name")]
        public async Task<IActionResult> GetGamesByName([FromQuery] string search)
        {
            var query = _context.Games.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(g => g.Name.Contains(search));
            }

            var games = await query.Select(g => new GameListItemDto
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

            return Ok(games);
        }

        [HttpGet("filter-features")]
        public async Task<IActionResult> GetGamesByFeatures([FromQuery] int? genreId, 
            [FromQuery] int? developerId, 
            [FromQuery] int? engineId, 
            [FromQuery] int? serieId, 
            [FromQuery] int? publisherId, 
            [FromQuery] int? modeId
        )
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

            var games = await query
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

            return Ok(games);

        }


        //[HttpGet("filter")]
        //public async Task<IActionResult> GetGamesFiltered([FromQuery] string? search, [FromQuery] int? genreId, [FromQuery] int? developerId, [FromQuery] int? engineId)
        //{
        //    var query = _context.Games
        //        .Include(g => g.GameGenres)
        //            .ThenInclude(gg => gg.Genre)
        //        .Include(g => g.GameDevelopers)
        //            .ThenInclude(gd => gd.Developer)
        //        .Include(g => g.GameEngines)
        //            .ThenInclude(ge => ge.Engine)
        //        .Include(g => g.GameImages)
        //        .AsQueryable();

        //    // Пошук за назвою
        //    if (!string.IsNullOrWhiteSpace(search))
        //    {
        //        query = query.Where(g => g.Name.Contains(search));
        //    }

        //    // Фільтри
        //    if (genreId.HasValue)
        //    {
        //        query = query.Where(g =>
        //            g.GameGenres.Any(gg => gg.GenreId == genreId));
        //    }

        //    if (developerId.HasValue)
        //    {
        //        query = query.Where(g =>
        //            g.GameDevelopers.Any(gd => gd.DeveloperId == developerId));
        //    }

        //    if (engineId.HasValue)
        //    {
        //        query = query.Where(g =>
        //            g.GameEngines.Any(ge => ge.EngineId == engineId));
        //    }

        //    var games = await query
        //        .Select(g => new GameListItemDto
        //        {
        //            Id = g.Id,
        //            Title = g.Name,
        //            Price = g.Price,
        //            MainImage = g.GameImages
        //                .Where(i => i.IsMain)
        //                .Select(i => i.ImagePath)
        //                .FirstOrDefault()
        //            //Genres = g.GameGenres
        //            //    .Select(gg => gg.Genre.Name)
        //            //    .ToList()
        //        })
        //        .ToListAsync();

        //    return Ok(games);
        //}

        //[HttpGet("{id:int}")]
        //public async Task<IActionResult> GetGameById(int id)
        //{
        //    var game = await _context.Games.Where(g => g.Id == id)
        //        .Select(g => new GameDetailsDto
        //        {
        //            Id = g.Id,
        //            Title = g.Name,
        //            Price = g.Price,
        //            ReleaseDate = g.ReleaseDate,
        //            Serie = g.Series != null ? g.Series.Name : null,
        //            Description = g.Description,
        //            Genres = g.GameGenres.Select(gg => gg.Genre.Name).ToList(),
        //            Developers = g.GameDevelopers.Select(gd => gd.Developer.Name).ToList(),
        //            Engines = g.GameEngines.Select(ge => ge.Engine.Name).ToList(),
        //            Publishers = g.GamePublishers.Select(gp => gp.Publisher.Name).ToList(),
        //            Modes = g.GameModes.Select(gm => gm.Mode.Name).ToList(),
        //            Requirements = g.SystemRequirements.Select(sr => new GameSystemRequirementsDto
        //            {
        //                Id = sr.Id,
        //                RequirementType = sr.RequirementType,
        //                Os = sr.Os,
        //                Cpu = sr.Cpu,
        //                Gpu = sr.Gpu,
        //                RamGb = sr.RamGb,
        //                StorageGb = sr.StorageGb,
        //                DirectX = sr.DirectX
        //            }).ToList(),
        //            Images = g.GameImages
        //                    .OrderBy(gi => gi.SortOrder)
        //                    .Select(gi => gi.ImagePath)
        //                    .ToList()
        //        })
        //        .FirstOrDefaultAsync();

        //    if (game == null) return NotFound();

        //    return Ok(game);

        //}

        [HttpGet("{gameId:int}/info")]
        public async Task<IActionResult> GetGameById(int gameId)
        {
            var game = await _context.Games.Where(g => g.Id == gameId)
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

            if (game == null) return NotFound();

            return Ok(game);
        }


        [HttpGet("{gameId:int}/requirements")]
        public async Task<IActionResult> GetGamesRequirements(int gameId)
        {
            var requirements = await _context.SystemRequirements.Where(sr => sr.GameId == gameId)
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

            if (requirements == null) return NotFound();

            return Ok(requirements);
        }

        [HttpGet("{gameId:int}/images")]
        public async Task<IActionResult> GetGamesImages(int gameId)
        {
            var images = await _context.GameImages.Where(gi => gi.GameId == gameId)
                .OrderBy(gi => gi.SortOrder)
                .Select(gi => gi.ImagePath)
                .ToListAsync();

            if (!images.Any()) return NotFound();

            return Ok(images);
        }
    }
}
