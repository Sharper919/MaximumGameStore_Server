namespace MaximumGameStore.DTOs
{
    public class UpdateUserPasswordDto
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
