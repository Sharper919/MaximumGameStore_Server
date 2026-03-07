namespace MaximumGameStore.DTOs
{
    public class CheckoutResponseDto
    {
        public int? OrderId { get; set; }
        public string ResponseMassage { get; set; } = null!;
    }
}
