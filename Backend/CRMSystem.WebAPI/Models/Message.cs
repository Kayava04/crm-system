namespace CRMSystem.WebAPI.Models
{
    public class Message
    {
        public Guid Id { get; private set; }
        public Guid ReceiverId { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public DateTime SentAt { get; private set; }
        public string Email { get; private set; }

        public Message(Guid id, Guid receiverId, string subject, string body, string email, DateTime sentAt)
        {
            Id = id;
            ReceiverId = receiverId;
            Subject = subject;
            Body = body;
            Email = email;
            SentAt = sentAt;
        }

        public static Message Create(Guid receiverId, string subject, string body, string email) =>
            new(Guid.NewGuid(), receiverId, subject, body, email, DateTime.UtcNow);
    }
}