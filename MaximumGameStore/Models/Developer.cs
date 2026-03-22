using MaximumGameStore.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class Developer : IGameFeatureEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GameDeveloper> GameDevelopers { get; set; } = new List<GameDeveloper>();
}
