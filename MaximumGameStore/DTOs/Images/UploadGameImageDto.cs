namespace MaximumGameStore.DTOs.Images
{
    public class UploadGameImageDto
    {
        public int GameId { get; set; }
        public IFormFile Image { get; set; } = null!;
        public bool IsMain { get; set; }
    }
}
