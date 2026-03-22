using MaximumGameStore.DTOs.GameDetails;
using MaximumGameStore.Models.Interfaces;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaximumGameStore.Controllers.GameFeatureControllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class GameFeatureController<T> : ControllerBase where T : class, IGameFeatureEntity
    {
        private readonly IGameFeatureService<T> _service;
        public GameFeatureController(IGameFeatureService<T> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var features = await _service.GetAll();
            return Ok(features);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameFeatureDto dto)
        {
            var id = await _service.Create(dto.Name);
            return Ok(id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, GameFeatureDto dto)
        {
            var result = await _service.Update(id, dto.Name);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
