using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class Publisher
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GamePublisher> GamePublishers { get; set; } = new List<GamePublisher>();
}
