using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class GameEngine
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public int EngineId { get; set; }

    public virtual Engine Engine { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;
}
