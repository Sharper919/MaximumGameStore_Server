using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class GameImage
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public string ImagePath { get; set; } = null!;

    public bool IsMain { get; set; }

    public int SortOrder { get; set; }

    public virtual Game Game { get; set; } = null!;
}
