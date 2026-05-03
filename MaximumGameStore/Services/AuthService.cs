using MaximumGameStore.Data;
using MaximumGameStore.DTOs;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Services
{
    public class AuthService : IAuthService
    {
        private readonly MaximumGameStoreContext _context;
        private readonly PasswordHasher _hasher;
        private readonly JwtService _jwt;

        public AuthService(MaximumGameStoreContext context, PasswordHasher hasher, JwtService jwt)
        {
            _context = context;
            _hasher = hasher;
            _jwt = jwt;
        }

        public async Task<AuthResponseDto?> AddUserAsync(RegisterDto dto)
        {
            string email = dto.Email.Trim().ToLower();
            string userName = dto.UserName.Trim();

            if (await _context.Users.AnyAsync(u => u.Email == email && !u.IsDeleted || u.Name == userName && !u.IsDeleted))
                return null;

            var user = new User
            {
                Email = email,
                Name = userName,
                PasswordHash = _hasher.Hash(dto.Password),
                DateTimeRegistration = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return MakeResponse(user);
        }

        public async Task<AuthResponseDto?> AuthenticateAsync(LoginDto dto)
        {
            string email = dto.Email.Trim().ToLower();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

            if (user == null || !_hasher.Verify(dto.Password, user.PasswordHash))
                return null;

            return MakeResponse(user);
        }

        private AuthResponseDto MakeResponse(User user)
        {
            var token = _jwt.CreateToken(user);

            var response = new AuthResponseDto
            {
                Token = token,
                UserName = user.Name,
                Role = user.Role
            };

            return response;
        }
    }
}
