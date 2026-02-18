using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Models;

public partial class Genre
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [InverseProperty("Genre")]
    public virtual ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
}
