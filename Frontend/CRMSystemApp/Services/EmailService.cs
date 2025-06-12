using CRMSystemApp.Models;

namespace CRMSystemApp.Services
{
    public static class EmailService
    {
        public static async Task<bool> SendMessageAsync(Guid receiverId, string subject, string body)
        {
            var message = new
            {
                ReceiverId = receiverId,
                Subject = subject,
                Body = body
            };

            var response = await ApiService.PostAsync("api/email/send", message);
            return response.IsSuccessStatusCode;
        }

        public static async Task<List<Message>> GetAllMessagesAsync()
        {
            return await ApiService.GetAsync<List<Message>>($"api/email/messages");
        }

        public static async Task<List<Message>> GetMessagesByReceiverIdAsync(Guid receiverId)
        {
            return await ApiService.GetAsync<List<Message>>($"api/email/messages/{receiverId}");
        }
    }
}