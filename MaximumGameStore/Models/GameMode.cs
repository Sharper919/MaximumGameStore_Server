using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class GameMode
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public int ModeId { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual Mode Mode { get; set; } = null!;
}
