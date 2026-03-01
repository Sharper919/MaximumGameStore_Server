using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Services.Interfaces;
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

        public async Task<string> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.IsDeleted) return "Not found";

            user.IsDeleted = true;
            await _context.SaveChangesAsync();

            return "Account deactivated";
        }

        public async Task<List<UserGamesDto>> GetUserGamesAsync(int userId)
        {
            return await _context.OrderItems.Where(oi => oi.Order.UserId == userId)
                .Select(oi => new UserGamesDto
                {
                    Id = oi.Id,
                    Title = oi.Game.Name,
                    Price = oi.PriceAtPurchase,
                    MainImage = oi.Game.GameImages.Where(gi => gi.IsMain)
                        .Select(gi => gi.ImagePath).FirstOrDefault(),
                    PurchasedAt = oi.Order.DateTimeOrder
                }).Distinct().ToListAsync();
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

        public async Task<string> UpdateUserNameAsync(int userId, UpdateUserNameDto dto)
        {
            string newUserName = dto.NewUserName.Trim();

            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.IsDeleted) return "Not found";

            if (string.IsNullOrWhiteSpace(newUserName))
                return "Username is empty";

            bool exists = await _context.Users
                .AnyAsync(u => u.Name == newUserName && u.Id != userId && !u.IsDeleted);

            if (exists) return "Username already taken";

            user.Name = newUserName;
            await _context.SaveChangesAsync();

            return "User name updated";
        }

        public async Task<string> UpdateUserPasswordAsync(int userId, UpdateUserPasswordDto dto)
        {
            PasswordHasher hasher = new PasswordHasher();

            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.IsDeleted) return "Not found";

            if (!hasher.Verify(dto.OldPassword, user.PasswordHash))
                return "Old password is incorrect";

            if (hasher.Verify(dto.NewPassword, user.PasswordHash))
                return "This is the old password";

            user.PasswordHash = hasher.Hash(dto.NewPassword);
            await _context.SaveChangesAsync();

            return "Password updated";
        }
    }
}
