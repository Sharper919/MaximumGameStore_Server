using MaximumGameStore.DTOs.GameDetails;
using MaximumGameStore.DTOs.GameFeature;
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
            var features = await _service.GetAllAsync();
            return Ok(features);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameFeatureNameDto dto)
        {
            var id = await _service.CreateAsync(dto.Name);
            return Ok(id);
        }
    }
}
