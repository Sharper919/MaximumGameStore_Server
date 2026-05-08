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

        public async Task<List<GameImage>?> UploadManyAsync(UploadGameImagesDto dto)
        {
            var gameExists = await _context.Games.AnyAsync(g => g.Id == dto.GameId && !g.IsDeleted);
            if (!gameExists) return null;

            if (dto.Images.Count == 0)
            {
                return new List<GameImage>();
            }

            var mainImageIndex = dto.MainImageIndex >= 0 && dto.MainImageIndex < dto.Images.Count
                ? dto.MainImageIndex
                : 0;

            var gameFolder = Path.Combine(_env.WebRootPath, "images", "games", dto.GameId.ToString());
            Directory.CreateDirectory(gameFolder);

            var oldMain = await _context.GameImages
                .Where(x => x.GameId == dto.GameId && x.IsMain)
                .ToListAsync();

            foreach (var img in oldMain)
                img.IsMain = false;

            var maxSort = await _context.GameImages
                .Where(x => x.GameId == dto.GameId)
                .MaxAsync(x => (int?)x.SortOrder) ?? 0;

            var uploadedImages = new List<GameImage>();

            for (var index = 0; index < dto.Images.Count; index++)
            {
                var file = dto.Images[index];
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fullPath = Path.Combine(gameFolder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                uploadedImages.Add(new GameImage
                {
                    GameId = dto.GameId,
                    ImagePath = $"/images/games/{dto.GameId}/{fileName}",
                    IsMain = index == mainImageIndex,
                    SortOrder = maxSort + index + 1
                });
            }

            _context.GameImages.AddRange(uploadedImages);
            await _context.SaveChangesAsync();

            return uploadedImages;
        }
    }
}
