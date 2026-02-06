using MaximumGameStore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace MaximumGameStore.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController2 : ControllerBase
    {
        private readonly MaximumGameStoreContext _context;

        public TestController2(MaximumGameStoreContext context)
        {
            _context = context;
        }

        [HttpGet("games")]
        public async Task<IActionResult> GetGames()
        {
            var games = await _context.Games.Take(5).ToListAsync();
            return Ok(games);
        }

    }
}
