using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Models;

[Index("OrderId", "GameId", Name = "UQ_Order_Game", IsUnique = true)]
public partial class OrderItem
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("OrderID")]
    public int OrderId { get; set; }

    [Column("GameID")]
    public int GameId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal PriceAtPurchase { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("OrderItems")]
    public virtual Game Game { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual Order Order { get; set; } = null!;
}
