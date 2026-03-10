using MaximumGameStore.DTOs;

namespace MaximumGameStore.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<(CheckoutResponseDto? dto, int statusCode, string massage)> CheckoutAsync(int userId, CheckoutDto dto);
        Task<(CheckoutResponseDto? dto, int statusCode, string massage)> BuyNowAsync(int userId, int gameId, CheckoutDto dto);
    }
}
