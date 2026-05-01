namespace MaximumGameStore.DTOs.Order
{
    public class AllOrdersDto
    {
        public int OrderId { get; set; }
        public string UserName { get; set; } = null!;
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = null!;
    }
}
