using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class CardPayment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string CardLast4 { get; set; } = null!;

    public string CardType { get; set; } = null!;

    public DateTime DateTimePayment { get; set; }

    public decimal Amount { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
