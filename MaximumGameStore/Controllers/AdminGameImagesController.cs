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

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] UploadGameImagesDto dto)
        {
            var result = await _service.UploadManyAsync(dto);
            if (result == null) return NotFound("Game not found");
            return Ok(result);
        }
    }
}
