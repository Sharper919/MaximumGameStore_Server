using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Models;

[Index("GameId", "EngineId", Name = "UQ_GameEngines", IsUnique = true)]
public partial class GameEngine
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("GameID")]
    public int GameId { get; set; }

    [Column("EngineID")]
    public int EngineId { get; set; }

    [ForeignKey("EngineId")]
    [InverseProperty("GameEngines")]
    public virtual Engine Engine { get; set; } = null!;

    [ForeignKey("GameId")]
    [InverseProperty("GameEngines")]
    public virtual Game Game { get; set; } = null!;
}
