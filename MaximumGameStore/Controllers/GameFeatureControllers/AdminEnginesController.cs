using MaximumGameStore.Controllers.GameFeatureControllers;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaximumGameStore.Controllers.GameDetailsControllers
{
    [Route("api/admin/engines")]
    public class AdminEnginesController : GameFeatureController<Engine>
    {
        public AdminEnginesController(IGameFeatureService<Engine> service) : base(service) { }
    }
}
