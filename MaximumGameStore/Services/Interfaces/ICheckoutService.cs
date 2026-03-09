using MaximumGameStore.DTOs;

namespace MaximumGameStore.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<CheckoutResponseDto> CheckoutAsync(int userId, CheckoutDto dto);
        Task<CheckoutResponseDto> BuyNowAsync(int userId, int gameId, CheckoutDto dto);
    }
}
