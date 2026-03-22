using MaximumGameStore.DTOs.Images;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaximumGameStore.Controllers
{
    [Route("api/admin/game-images")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminGameImagesController : ControllerBase
    {
        private readonly IGameImageService _service;

        public AdminGameImagesController(IGameImageService service)
        {
            _service = service;
        }

        [HttpGet("game/{gameId:int}")]
        public async Task<IActionResult> GetByGame(int gameId)
        {
            var images = await _service.GetByGameId(gameId);
            return Ok(images);
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] UploadGameImageDto dto)
        {
            var result = await _service.Upload(dto);
            if (result == null) return NotFound("Game not found");
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.Delete(id);
            return success ? Ok() : NotFound();
        }

        [HttpPut("{id:int}/set-main")]
        public async Task<IActionResult> SetMain(int id)
        {
            var success = await _service.SetMain(id);
            return success ? Ok() : NotFound();
        }
    }
}
