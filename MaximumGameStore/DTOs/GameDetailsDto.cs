namespace MaximumGameStore.DTOs
{
    public class GameDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        //public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public string? Serie {  get; set; }

        public List<string> Genres { get; set; } = new();
        public List<string> Developers { get; set; } = new();
        public List<string> Engines { get; set; } = new();
        public List<string> Publishers { get; set; } = new();
        public List<string> Modes { get; set; } = new();

        public List<GameSystemRequirementsDto> Requirements { get; set; } = new();

        public List<string> Images { get; set; } = new();

    }
}
