namespace MaximumGameStore.DTOs.Game
{
    public class GameAdminListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string? MainImage { get; set; }
        public DateOnly? ReleaseDate { get; set; }
    }
}
