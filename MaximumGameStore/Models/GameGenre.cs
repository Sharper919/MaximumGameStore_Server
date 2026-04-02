using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Models;

[Index("GameId", "GenreId", Name = "UQ_GameGenres", IsUnique = true)]
public partial class GameGenre
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("GameID")]
    public int GameId { get; set; }

    [Column("GenreID")]
    public int GenreId { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("GameGenres")]
    public virtual Game Game { get; set; } = null!;

    [ForeignKey("GenreId")]
    [InverseProperty("GameGenres")]
    public virtual Genre Genre { get; set; } = null!;
}
