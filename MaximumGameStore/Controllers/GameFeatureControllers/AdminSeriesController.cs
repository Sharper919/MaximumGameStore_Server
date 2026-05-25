using MaximumGameStore.Controllers.GameFeatureControllers;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaximumGameStore.Controllers.GameDetailsControllers
{
    [Route("api/admin/series")]
    public class AdminSeriesController : GameFeatureController<Series>
    {
        public AdminSeriesController(IGameFeatureService<Series> service) : base(service) { }
    }
}
