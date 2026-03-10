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
            
            if (result.statusCode == 400)
                return BadRequest(result.massage);

            if (result.statusCode == 500)
                return StatusCode(500, result.massage);

            return Ok(result.dto);
        }

        [HttpPost("buy-now/{gameId:int}")]
        public async Task<IActionResult> BuyNow(int gameId, CheckoutDto dto)
        {
            int userId = GetUserId();

            var result = await _checkoutService.BuyNowAsync(userId, gameId, dto);

            if (result.statusCode == 400)
                return BadRequest(result.massage);

            if (result.statusCode == 404)
                return BadRequest(result.massage);

            if (result.statusCode == 500)
                return StatusCode(500, result.massage);

            return Ok(result.dto);
        }

        private int GetUserId()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(Id!);
        }
    }
}
