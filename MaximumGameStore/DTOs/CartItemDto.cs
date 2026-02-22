namespace MaximumGameStore.DTOs
{
    public class CartItemDto
    {
        public int GameId { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}
