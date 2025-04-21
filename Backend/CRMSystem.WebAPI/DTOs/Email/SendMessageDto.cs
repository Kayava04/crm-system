namespace CRMSystem.WebAPI.DTOs.Email
{
    public class SendMessageDto
    {
        public Guid ReceiverId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}