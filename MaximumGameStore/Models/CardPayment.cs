using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Models;

[Index("OrderId", Name = "UQ__CardPaym__C3905BAE08388269", IsUnique = true)]
public partial class CardPayment
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("OrderID")]
    public int OrderId { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string CardLast4 { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string CardType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DateTimePayment { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PaymentStatus { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("CardPayment")]
    public virtual Order Order { get; set; } = null!;
}
