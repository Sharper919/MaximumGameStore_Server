using MaximumGameStore.DTOs;

namespace MaximumGameStore.Services.Interfaces
{
    public interface IUserService
    {
        // user functions
        Task<UserInfoDto?> GetUserInfoAsync(int userId);
        Task<List<UserGamesDto>> GetUserGamesAsync(int userId);
        Task<(string massage, int statusCode)> UpdateUserNameAsync(int userId, UpdateUserNameDto dto);
        Task<(string massage, int statusCode)> UpdateUserPasswordAsync(int userId, UpdateUserPasswordDto dto);
        Task<(string massage, int statusCode)> DeleteUserAsync(int userId);

        // admin functions
        Task<List<UserInfoDto>> GetUsersAsync();
        Task<bool> BlockUserAsync(int userId);
    }
}
