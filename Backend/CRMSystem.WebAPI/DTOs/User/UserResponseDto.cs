namespace CRMSystem.WebAPI.DTOs.User
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? ImageUrl { get; set; }
    }
}