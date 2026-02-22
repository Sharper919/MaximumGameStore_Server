namespace MaximumGameStore.DTOs
{
    public class CartDto
    {
        public int CartId { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public decimal TotalPrice { get; set; }
    }
}
