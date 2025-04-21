using AutoMapper;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Repositories
{
    public class ContactRepository(SchoolDbContext context, IMapper mapper)
        : IBasicRepository<Contact>
    {
        public async Task<Contact?> GetByIdAsync(Guid id)
        {
            var entity = await context.Contacts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            
            return mapper.Map<Contact>(entity);
        }

        public async Task<IEnumerable<Contact>> GetAllAsync(IFilterDto? filter = null)
        {
            var entities = await context.Contacts
                .AsNoTracking()
                .ToListAsync();
            
            return mapper.Map<IEnumerable<Contact>>(entities);
        }

        public async Task<Contact> AddAsync(Contact contact)
        {
            var entity = mapper.Map<ContactEntity>(contact);
            await context.Contacts.AddAsync(entity);
            
            await context.SaveChangesAsync();
            return mapper.Map<Contact>(entity);
        }

        public async Task<Contact?> UpdateAsync(Guid id, Contact contact)
        {
            var existing = await context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (existing == null) return null;

            context.Entry(existing)
                .CurrentValues.SetValues(mapper.Map<ContactEntity>(contact));
            
            await context.SaveChangesAsync();
            return mapper.Map<Contact>(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await context.Contacts.
                FirstOrDefaultAsync(c => c.Id == id);
            
            if (entity != null)
            {
                context.Contacts.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}