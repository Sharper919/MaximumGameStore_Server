using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Models;

public partial class Game
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Column("SeriesID")]
    public int? SeriesId { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    public string? Description { get; set; }

    public bool IsDeleted { get; set; }

    [InverseProperty("Game")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [InverseProperty("Game")]
    public virtual ICollection<GameDeveloper> GameDevelopers { get; set; } = new List<GameDeveloper>();

    [InverseProperty("Game")]
    public virtual ICollection<GameEngine> GameEngines { get; set; } = new List<GameEngine>();

    [InverseProperty("Game")]
    public virtual ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

    [InverseProperty("Game")]
    public virtual ICollection<GameImage> GameImages { get; set; } = new List<GameImage>();

    [InverseProperty("Game")]
    public virtual ICollection<GameMode> GameModes { get; set; } = new List<GameMode>();

    [InverseProperty("Game")]
    public virtual ICollection<GamePublisher> GamePublishers { get; set; } = new List<GamePublisher>();

    [InverseProperty("Game")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("SeriesId")]
    [InverseProperty("Games")]
    public virtual Series? Series { get; set; }

    [InverseProperty("Game")]
    public virtual ICollection<SystemRequirement> SystemRequirements { get; set; } = new List<SystemRequirement>();
}
