using System.Net;
using System.Net.Mail;
using AutoMapper;
using CRMSystem.WebAPI.DTOs.Email;
using CRMSystem.WebAPI.Email;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.Extensions.Options;

namespace CRMSystem.WebAPI.Services
{
    public class EmailService(
        IEmailRepository messageRepository,
        IMapper mapper,
        IOptions<SmtpSettings> smtpSettings,
        IOptions<EmailCredentials> emailCredentials)
    {
        private readonly SmtpSettings _smtpSettings = smtpSettings.Value;
        private readonly EmailCredentials _emailCredentials = emailCredentials.Value;

        public async Task<MessageDto> SendMessageAsync(SendMessageDto dto)
        {
            var message = Message.Create(dto.ReceiverId, dto.Subject, dto.Body, string.Empty);
            var savedMessage = await messageRepository.AddAsync(message);
            
            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port);
            client.Credentials = new NetworkCredential(_emailCredentials.Email, _emailCredentials.Password);
            client.EnableSsl = true;

            var mailMessage = new MailMessage(_emailCredentials.Email, savedMessage.Email, dto.Subject, dto.Body);
            await client.SendMailAsync(mailMessage);

            return mapper.Map<MessageDto>(savedMessage);
        }

        public async Task<IEnumerable<MessageDto>> GetAllMessagesAsync()
        {
            var messages = await messageRepository.GetAllMessagesAsync();
            
            return mapper.Map<IEnumerable<MessageDto>>(messages);
        }
        
        public async Task<IEnumerable<MessageDto>> GetMessagesByReceiverIdAsync(Guid receiverId)
        {
            var messages = await messageRepository.GetMessagesByReceiverIdAsync(receiverId);
            
            return mapper.Map<IEnumerable<MessageDto>>(messages);
        }
    }
}