using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Models;

public partial class GameImage
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("GameID")]
    public int GameId { get; set; }

    [StringLength(500)]
    public string ImagePath { get; set; } = null!;

    public bool IsMain { get; set; }

    public int SortOrder { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("GameImages")]
    public virtual Game Game { get; set; } = null!;
}
