namespace CRMSystem.WebAPI.Entities.Email
{
    public class MessageEntity
    {
        public Guid Id { get; set; }
        public Guid ReceiverId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}