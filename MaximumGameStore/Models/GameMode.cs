using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Models;

[Index("GameId", "ModeId", Name = "UQ_GameModes", IsUnique = true)]
public partial class GameMode
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("GameID")]
    public int GameId { get; set; }

    [Column("ModeID")]
    public int ModeId { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("GameModes")]
    public virtual Game Game { get; set; } = null!;

    [ForeignKey("ModeId")]
    [InverseProperty("GameModes")]
    public virtual Mode Mode { get; set; } = null!;
}
