using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class GamePublisher
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public int PublisherId { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual Publisher Publisher { get; set; } = null!;
}
