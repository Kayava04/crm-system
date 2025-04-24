namespace CRMSystem.WebAPI.DTOs.User
{
    public class UpdateUserDto
    {
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Email { get; set; }
    }
}