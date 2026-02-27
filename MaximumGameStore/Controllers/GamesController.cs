using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;
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
        private readonly IGameService _gameService;
        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGames()
        {
            return Ok(await _gameService.GetGamesAsync());
        }

        [HttpGet("filter-name")]
        public async Task<IActionResult> GetGamesByName([FromQuery] string search)
        {
            return Ok(await _gameService.GetGamesByNameAsync(search));
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
            return Ok(await _gameService.FilterAsync(genreId, developerId, engineId, serieId, publisherId, modeId));
        }

        [HttpGet("{gameId:int}/info")]
        public async Task<IActionResult> GetGameDetailsById(int gameId)
        {
            var game = await _gameService.GetGameDetailsByIdAsync(gameId);

            if (game == null) return NotFound();

            return Ok(game);
        }

        [HttpGet("{gameId:int}/requirements")]
        public async Task<IActionResult> GetGamesRequirements(int gameId)
        {
            return Ok(await _gameService.GetGamesRequirementsByIdAsync(gameId));
        }

        [HttpGet("{gameId:int}/images")]
        public async Task<IActionResult> GetGamesImages(int gameId)
        {
            return Ok(await _gameService.GetGameImagesByIdAsync(gameId));
        }
    }
}
