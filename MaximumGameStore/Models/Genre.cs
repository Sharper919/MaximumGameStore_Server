using MaximumGameStore.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class Genre : IGameFeatureEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
}
