using MaximumGameStore.DTOs.GameDetails;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaximumGameStore.Controllers.GameDetailsControllers
{
    [Route("api/admin/genres")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminGenresController : ControllerBase
    {
        private readonly IGameFeaturesService _service;

        public AdminGenresController(IGameFeaturesService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(GameFeaturesDto dto)
        {
            var developerId = await _service.Create(dto.Name);
            return Ok(developerId);
        }

        [HttpPut("{id:int}/update")]
        public async Task<IActionResult> Update(int id, GameFeaturesDto dto)
        {
            var result = await _service.Update(id, dto.Name);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
