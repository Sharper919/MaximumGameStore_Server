using MaximumGameStore.Data;
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

        [HttpGet("/")]
        public async Task<IActionResult> GetGamesList()
        {
            var games = await _context.Games
                .Join(_context.GameImages,
                    g => g.Id,
                    gi => gi.GameId,
                    (g, gi) => new
                    {
                        g.Id,
                        g.Name,
                        g.Price,
                        gi.ImagePath
                    }).Take(8).ToListAsync();
            return Ok(games);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGamesList(int id)
        {
            var games = await _context.Games
                .Join(_context.GameImages,
                    g => g.Id,
                    gi => gi.GameId,
                    (g, gi) => new
                    {
                        g.Id,
                        g.Name,
                        g.Price,
                        gi.ImagePath
                    }).Take(8).ToListAsync();
            return Ok(games);
        }

    }
}
