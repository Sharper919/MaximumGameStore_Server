using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class Mode
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GameMode> GameModes { get; set; } = new List<GameMode>();
}
