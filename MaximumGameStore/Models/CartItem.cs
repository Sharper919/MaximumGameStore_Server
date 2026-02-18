using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Models;

[Index("CartId", "GameId", Name = "UQ_Cart_Game", IsUnique = true)]
public partial class CartItem
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("CartID")]
    public int CartId { get; set; }

    [Column("GameID")]
    public int GameId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateTimeAdded { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartItems")]
    public virtual Cart Cart { get; set; } = null!;

    [ForeignKey("GameId")]
    [InverseProperty("CartItems")]
    public virtual Game Game { get; set; } = null!;
}
