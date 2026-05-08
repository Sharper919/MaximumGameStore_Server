namespace MaximumGameStore.DTOs.Images
{
    public class UploadGameImagesDto
    {
        public int GameId { get; set; }
        public List<IFormFile> Images { get; set; } = new();
        public int MainImageIndex { get; set; }
    }
}
