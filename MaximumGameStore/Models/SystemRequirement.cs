using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Models;

[Index("GameId", "RequirementType", Name = "UQ_RequirementType_Game", IsUnique = true)]
public partial class SystemRequirement
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("GameID")]
    public int GameId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string RequirementType { get; set; } = null!;

    [Column("OS")]
    [StringLength(150)]
    public string Os { get; set; } = null!;

    [Column("CPU")]
    [StringLength(150)]
    public string Cpu { get; set; } = null!;

    [Column("GPU")]
    [StringLength(150)]
    public string Gpu { get; set; } = null!;

    [Column("RAM_GB")]
    public int RamGb { get; set; }

    [Column("Storage_GB")]
    public int StorageGb { get; set; }

    [StringLength(50)]
    public string? DirectX { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("SystemRequirements")]
    public virtual Game Game { get; set; } = null!;
}
