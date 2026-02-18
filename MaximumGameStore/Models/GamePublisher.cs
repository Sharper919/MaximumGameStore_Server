using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Models;

[Index("GameId", "PublisherId", Name = "UQ_GamePublishers", IsUnique = true)]
public partial class GamePublisher
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("GameID")]
    public int GameId { get; set; }

    [Column("PublisherID")]
    public int PublisherId { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("GamePublishers")]
    public virtual Game Game { get; set; } = null!;

    [ForeignKey("PublisherId")]
    [InverseProperty("GamePublishers")]
    public virtual Publisher Publisher { get; set; } = null!;
}
