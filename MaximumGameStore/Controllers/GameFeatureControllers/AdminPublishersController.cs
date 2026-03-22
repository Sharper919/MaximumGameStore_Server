using MaximumGameStore.Controllers.GameFeatureControllers;
using MaximumGameStore.DTOs.GameDetails;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaximumGameStore.Controllers.GameDetailsControllers
{
    [Route("api/admin/publishers")]
    public class AdminPublishersController : GameFeatureController<Publisher>
    {
        public AdminPublishersController(IGameFeatureService<Publisher> service) : base(service) { }
    }
}
