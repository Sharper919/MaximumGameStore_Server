using MaximumGameStore.Controllers.GameFeatureControllers;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaximumGameStore.Controllers.GameDetailsControllers
{
    [Route("api/admin/developers")]
    public class AdminDevelopersController : GameFeatureController<Developer>
    {
        public AdminDevelopersController(IGameFeatureService<Developer> service) : base(service) { }
    }
}
