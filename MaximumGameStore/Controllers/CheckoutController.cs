using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MaximumGameStore.Controllers
{
    [Route("api/checkout")]
    [ApiController]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutDto dto)
        {
            int userId = GetUserId();

            var result = await _checkoutService.CheckoutAsync(userId, dto);
            
            if (result.OrderId == null)
                return BadRequest(result.ResponseMassage);

            return Ok(result);
        }

        [HttpPost("buy-now/{gameId:int}")]
        public async Task<IActionResult> BuyNow(int gameId, CheckoutDto dto)
        {
            int userId = GetUserId();

            var result = await _checkoutService.BuyNowAsync(userId, gameId, dto);

            if (result.OrderId == null)
                return BadRequest(result.ResponseMassage);

            return Ok(result);
        }

        private int GetUserId()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(Id!);
        }
    }
}
