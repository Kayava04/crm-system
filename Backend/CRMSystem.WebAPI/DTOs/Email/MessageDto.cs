namespace CRMSystem.WebAPI.DTOs.Email
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid ReceiverId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SentAt { get; set; }
        public string Email { get; set; }
    }
}