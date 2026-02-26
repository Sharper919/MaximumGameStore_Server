namespace MaximumGameStore.DTOs
{
    public class GameListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string? MainImage { get; set; }
    }
}
