namespace MaximumGameStore.DTOs
{
    public class GameSystemRequirementsDto
    {
        public int Id { get; set; }
        public string RequirementType { get; set; } = null!;
        public string Os { get; set; } = null!;
        public string Cpu { get; set; } = null!;
        public string Gpu { get; set; } = null!;
        public int RamGb { get; set; }
        public int StorageGb { get; set; }
        public string? DirectX { get; set; }
    }
}
