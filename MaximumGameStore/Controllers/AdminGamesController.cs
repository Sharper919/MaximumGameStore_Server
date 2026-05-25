using MaximumGameStore.DTOs;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaximumGameStore.Controllers
{
    [Route("api/admin/games")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminGamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public AdminGamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGames()
        {
            return Ok(await _gameService.GetAllGamesAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddGame(CreateGameDto dto)
        {
            var gameId = await _gameService.CreateGameAsync(dto);

            if (gameId == null) return BadRequest("Failed to create game");

            return Ok(gameId);
        }

        [HttpPut("{gameId:int}/delete")]
        public async Task<IActionResult> DeleteGame(int gameId)
        {
            var result = await _gameService.DeleteGameAsync(gameId);

            if (!result) return NotFound();

            return NoContent();
        }
    }
}
