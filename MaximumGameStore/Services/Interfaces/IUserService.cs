using MaximumGameStore.DTOs;

namespace MaximumGameStore.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserInfoDto?> GetUserInfoAsync(int userId);
        Task<List<UserGamesDto>> GetUserGamesAsync(int userId);
        Task<string> UpdateUserNameAsync(int userId, UpdateUserNameDto dto);
        Task<string> UpdateUserPasswordAsync(int userId, UpdateUserPasswordDto dto);
        Task<string> DeleteUserAsync(int userId);
    }
}
