using MaximumGameStore.Controllers.GameFeatureControllers;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaximumGameStore.Controllers.GameDetailsControllers
{
    [Route("api/admin/modes")]
    public class AdminModesController : GameFeatureController<Mode>
    {
        public AdminModesController(IGameFeatureService<Mode> service) : base(service) { }
    }
}
