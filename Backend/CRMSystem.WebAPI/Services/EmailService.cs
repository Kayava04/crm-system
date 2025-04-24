using AutoMapper;
using CRMSystem.WebAPI.DTOs.Email;
using CRMSystem.WebAPI.Email;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

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

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailCredentials.Email));
            email.To.Add(MailboxAddress.Parse(savedMessage.Email));
            email.Subject = dto.Subject;
            email.Body = new TextPart("plain") { Text = dto.Body };

            try
            {
                using var smtp = new SmtpClient();
                
                //-------------------- OPTIONS --------------------\\
                // SecureSocketOptions.SslOnConnect - 465 Port
                // SecureSocketOptions.StartTls - 587 Port
                
                await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(_emailCredentials.Email, _emailCredentials.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"SMTP error: {ex.Message} (Host: {_smtpSettings.Host}, Port: {_smtpSettings.Port})", ex);
            }

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