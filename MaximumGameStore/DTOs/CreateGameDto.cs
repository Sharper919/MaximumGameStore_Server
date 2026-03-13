namespace MaximumGameStore.DTOs
{
    public class CreateGameDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public int? SeriesId { get; set; }
        public List<int> GenreIds { get; set; } = new();
        public List<int> DeveloperIds { get; set; } = new();
        public List<int> PublisherIds { get; set; } = new();
        public List<int> ModeIds { get; set; } = new();
        public List<int> EngineIds { get; set; } = new();
    }
}
