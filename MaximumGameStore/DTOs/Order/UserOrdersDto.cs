namespace MaximumGameStore.DTOs.Order
{
    public class UserOrdersDto
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = null!;
    }
}
