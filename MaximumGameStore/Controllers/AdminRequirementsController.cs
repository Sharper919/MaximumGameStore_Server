using MaximumGameStore.DTOs;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaximumGameStore.Controllers
{
    [Route("api/admin/requirements")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminRequirementsController : ControllerBase
    {
        private readonly ISystemRequirementService _requirementService;

        public AdminRequirementsController(ISystemRequirementService requirementService)
        {
            _requirementService = requirementService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRequirement(AddRequirementsDto dto)
        {
            var requirementId = await _requirementService.AddRequirementsAsync(dto.GameId, dto);
            return Ok(requirementId);
        }
    }
}
