namespace CRMSystemApp.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SentAt { get; set; }
        public string Email {  get; set; }
        public int RowNumber { get; set; }
    }
}