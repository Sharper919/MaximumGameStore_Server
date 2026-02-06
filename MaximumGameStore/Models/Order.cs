using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime DateTimeOrder { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public virtual CardPayment? CardPayment { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User User { get; set; } = null!;
}
