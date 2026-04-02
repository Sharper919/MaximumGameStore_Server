using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Models;

[Index("GameId", "DeveloperId", Name = "UQ_GameDeveloper", IsUnique = true)]
public partial class GameDeveloper
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("GameID")]
    public int GameId { get; set; }

    [Column("DeveloperID")]
    public int DeveloperId { get; set; }

    [ForeignKey("DeveloperId")]
    [InverseProperty("GameDevelopers")]
    public virtual Developer Developer { get; set; } = null!;

    [ForeignKey("GameId")]
    [InverseProperty("GameDevelopers")]
    public virtual Game Game { get; set; } = null!;
}
