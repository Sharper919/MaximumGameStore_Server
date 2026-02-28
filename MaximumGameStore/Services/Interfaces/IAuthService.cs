using MaximumGameStore.DTOs;
using MaximumGameStore.Models;

namespace MaximumGameStore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> AddUserAsync(RegisterDto dto);
        Task<AuthResponseDto?> AuthenticateAsync(LoginDto dto);
    }
}
