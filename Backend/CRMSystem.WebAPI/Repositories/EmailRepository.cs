using AutoMapper;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.Entities.Email;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Repositories
{
    public class EmailRepository(SchoolDbContext context, IMapper mapper)
        : IEmailRepository
    {
        public async Task<Message> AddAsync(Message message)
        {
            string? email;
            
            var student = await context.Students
                .Include(s => s.Person)
                .ThenInclude(p => p.Contacts)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == message.ReceiverId);
            
            if (student != null)
                email = student.Person?.Contacts?.FirstOrDefault(c => !string.IsNullOrEmpty(c.Email))?.Email;
            else
            {
                var employee = await context.Employees
                    .Include(e => e.Person)
                    .ThenInclude(p => p.Contacts)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == message.ReceiverId);
            
                email = employee?.Person?.Contacts?.FirstOrDefault(c => !string.IsNullOrEmpty(c.Email))?.Email;
            }
            
            var messageWithEmail = Message.Create(message.ReceiverId, message.Subject, message.Body, email);
            
            var entity = mapper.Map<MessageEntity>(messageWithEmail);
            await context.Messages.AddAsync(entity);
            
            await context.SaveChangesAsync();
            return mapper.Map<Message>(entity);
        }

        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            var entities = await context.Messages
                .AsNoTracking()
                .ToListAsync();
            
            return mapper.Map<IEnumerable<Message>>(entities);
        }

        public async Task<IEnumerable<Message>> GetMessagesByReceiverIdAsync(Guid receiverId)
        {
            var entities = await context.Messages
                .Where(m => m.ReceiverId == receiverId)
                .ToListAsync();
            
            return mapper.Map<IEnumerable<Message>>(entities);
        }
    }
}