using AutoMapper;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Repositories
{
    public class PersonRepository(SchoolDbContext context, IMapper mapper)
        : IBasicRepository<Person>
    {
        public async Task<Person?> GetByIdAsync(Guid id)
        {
            var entity = await context.Persons
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            
            return mapper.Map<Person>(entity);
        }

        public async Task<IEnumerable<Person>> GetAllAsync(IFilterDto? filter = null)
        {
            var entities = await context.Persons
                .AsNoTracking()
                .ToListAsync();
            
            return mapper.Map<IEnumerable<Person>>(entities);
        }

        public async Task<Person> AddAsync(Person person)
        {
            var entity = mapper.Map<PersonEntity>(person);
            await context.Persons.AddAsync(entity);
            
            await context.SaveChangesAsync();
            return mapper.Map<Person>(entity);
        }

        public async Task<Person?> UpdateAsync(Guid id, Person person)
        {
            var existing = await context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (existing == null) return null;

            context.Entry(existing)
                .CurrentValues.SetValues(mapper.Map<PersonEntity>(person));
            
            await context.SaveChangesAsync();
            return mapper.Map<Person>(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (entity != null)
            {
                context.Persons.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}