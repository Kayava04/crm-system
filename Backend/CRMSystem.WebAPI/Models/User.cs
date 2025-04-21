namespace CRMSystem.WebAPI.Models
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string Email { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public int RoleId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string? ImageUrl { get; private set; }

        private User(Guid id, string fullname, DateTime birthDate, string email, string username, string passwordHash, int roleId)
        {
            Id = id;
            FullName = fullname;
            BirthDate = birthDate;
            Email = email;
            Username = username;
            PasswordHash = passwordHash;
            RoleId = roleId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            ImageUrl = null;
        }
    
        public static User Create(Guid id, string fullname, DateTime birthDate, string email, string username, string passwordHash, int roleId) =>
            new(id, fullname, birthDate, email, username, passwordHash, roleId);

        public void Update(string fullName, DateTime birthDate, string email, string? passwordHash = null)
        {
            FullName = fullName;
            BirthDate = birthDate;
            Email = email;
            UpdatedAt = DateTime.UtcNow;
            
            if (!string.IsNullOrWhiteSpace(passwordHash))
                PasswordHash = passwordHash;
        }
        
        public void TouchUpdatedAt() => UpdatedAt = DateTime.UtcNow;
        
        public void SetImageUrl(string imageUrl) => ImageUrl = imageUrl;
    }
}