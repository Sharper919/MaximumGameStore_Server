namespace MaximumGameStore.DTOs
{
    public class UserGamesDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? MainImage { get; set; } = null!;
        public List<string> Genres { get; set; } = new List<string>();
    }
}
