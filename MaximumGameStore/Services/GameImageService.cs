using MaximumGameStore.Data;
using MaximumGameStore.DTOs.Images;
using MaximumGameStore.Models;
using MaximumGameStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Services
{
    public class GameImageService : IGameImageService
    {
        private readonly MaximumGameStoreContext _context;
        private readonly IWebHostEnvironment _env;

        public GameImageService(MaximumGameStoreContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<List<GameImage>> GetByGameId(int gameId)
        {
            return await _context.GameImages
                .Where(x => x.GameId == gameId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();
        }

        public async Task<GameImage?> Upload(UploadGameImageDto dto)
        {
            var gameExists = await _context.Games.AnyAsync(g => g.Id == dto.GameId);
            if (!gameExists) return null;

            // шлях до папки гри
            var gameFolder = Path.Combine(_env.WebRootPath, "images", "games", dto.GameId.ToString());
            Directory.CreateDirectory(gameFolder);

            // ім'я файлу
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
            var fullPath = Path.Combine(gameFolder, fileName);

            // збереження файлу
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            // якщо головне — скидаємо попереднє
            if (dto.IsMain)
            {
                var oldMain = await _context.GameImages
                    .Where(x => x.GameId == dto.GameId && x.IsMain)
                    .ToListAsync();

                foreach (var img in oldMain)
                    img.IsMain = false;
            }

            // порядок сортування
            var maxSort = await _context.GameImages
                .Where(x => x.GameId == dto.GameId)
                .MaxAsync(x => (int?)x.SortOrder) ?? 0;

            var image = new GameImage
            {
                GameId = dto.GameId,
                ImagePath = $"/images/games/{dto.GameId}/{fileName}",
                IsMain = dto.IsMain,
                SortOrder = maxSort + 1
            };

            _context.GameImages.Add(image);
            await _context.SaveChangesAsync();

            return image;
        }

        public async Task<bool> Delete(int imageId)
        {
            var image = await _context.GameImages.FindAsync(imageId);
            if (image == null) return false;

            var fullPath = Path.Combine(_env.WebRootPath, image.ImagePath.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            _context.GameImages.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetMain(int imageId)
        {
            var image = await _context.GameImages.FindAsync(imageId);
            if (image == null) return false;

            var images = await _context.GameImages
                .Where(x => x.GameId == image.GameId)
                .ToListAsync();

            foreach (var img in images)
                img.IsMain = false;

            image.IsMain = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
