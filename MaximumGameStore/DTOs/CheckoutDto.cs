namespace MaximumGameStore.DTOs
{
    public class CheckoutDto
    {
        public string CardNumber { get; set; } = null!;
        public string CardType { get; set; } = null!;
        public string Expiry { get; set; } = null!;
        public string CVV { get; set; } = null!;
    }
}
