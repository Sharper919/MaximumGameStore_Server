using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class Engine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GameEngine> GameEngines { get; set; } = new List<GameEngine>();
}
