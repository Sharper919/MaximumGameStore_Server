namespace MaximumGameStore.DTOs
{
    public class UpdateUserDto
    {
        public string UserName { get; set; } = null!;
        public string? NewPassword { get; set; }
    }
}
