namespace CRMSystem.WebAPI.Entities.Auth
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? ImageUrl { get; set; }
        
        public RoleEntity Role { get; set; } = null!;
    }
}