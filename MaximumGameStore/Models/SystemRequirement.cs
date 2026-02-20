using System;
using System.Collections.Generic;

namespace MaximumGameStore.Models;

public partial class SystemRequirement
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public string RequirementType { get; set; } = null!;

    public string Os { get; set; } = null!;

    public string Cpu { get; set; } = null!;

    public string Gpu { get; set; } = null!;

    public int RamGb { get; set; }

    public int StorageGb { get; set; }

    public string? DirectX { get; set; }

    public virtual Game Game { get; set; } = null!;
}
