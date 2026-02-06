using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime DateTimeCreation { get; set; }

    public DateTime DateTimeUpdate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User User { get; set; } = null!;
}
