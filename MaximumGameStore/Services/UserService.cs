using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Services
{
    public class UserService : IUserService
    {
        private readonly MaximumGameStoreContext _context;

        public UserService(MaximumGameStoreContext context)
        {
            _context = context;
        }

        // user functions
        public async Task<(string massage, int statusCode)> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.IsDeleted) return ("Not found", 404);

            user.IsDeleted = true;
            await _context.SaveChangesAsync();

            return ("Account deactivated", 200);
        }

        public async Task<List<UserGamesDto>> GetUserGamesAsync(int userId)
        {
            return await _context.OrderItems.Where(oi => oi.Order.UserId == userId)
                .Select(oi => new UserGamesDto
                {
                    Id = oi.GameId,
                    Title = oi.Game.Name,
                    MainImage = oi.Game.GameImages.Where(gi => gi.IsMain)
                        .Select(gi => gi.ImagePath).FirstOrDefault(),
                    Genres = oi.Game.GameGenres.Select(gg => gg.Genre.Name).ToList()
                }).ToListAsync();
        }

        public async Task<UserInfoDto?> GetUserInfoAsync(int userId)
        {
            return await _context.Users.Where(u => u.Id == userId)
                .Select(ui => new UserInfoDto
                {
                    Id = ui.Id,
                    UserName = ui.Name,
                    Email = ui.Email,
                    CreatedAt = ui.DateTimeRegistration
                }).FirstOrDefaultAsync();
        }

        public async Task<(string massage, int statusCode)> UpdateUserNameAsync(int userId, UpdateUserNameDto dto)
        {
            string newUserName = dto.NewUserName.Trim();

            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.IsDeleted) return ("Not found", 404);

            if (string.IsNullOrWhiteSpace(newUserName))
                return ("Username is empty", 400);

            bool exists = await _context.Users
                .AnyAsync(u => u.Name == newUserName && u.Id != userId && !u.IsDeleted);

            if (exists) return ("Username already taken", 400);

            user.Name = newUserName;
            await _context.SaveChangesAsync();

            return ("User name updated", 200);
        }

        public async Task<(string massage, int statusCode)> UpdateUserPasswordAsync(int userId, UpdateUserPasswordDto dto)
        {
            PasswordHasher hasher = new PasswordHasher();

            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.IsDeleted) return ("Not found", 404);

            if (!hasher.Verify(dto.OldPassword, user.PasswordHash))
                return ("Old password is incorrect", 400);

            if (hasher.Verify(dto.NewPassword, user.PasswordHash))
                return ("This is the old password", 400);

            user.PasswordHash = hasher.Hash(dto.NewPassword);
            await _context.SaveChangesAsync();

            return ("Password updated", 200);
        }

        // admin functions
        public async Task<List<UserInfoDto>> GetUsersAsync()
        {
            return await _context.Users.Where(u => !u.IsDeleted)
                .Select(u => new UserInfoDto
                {
                    Id = u.Id,
                    UserName = u.Name,
                    Email = u.Email,
                    CreatedAt = u.DateTimeRegistration
                }).ToListAsync();
        }

        public async Task<bool> BlockUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.IsDeleted) return false;

            user.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
