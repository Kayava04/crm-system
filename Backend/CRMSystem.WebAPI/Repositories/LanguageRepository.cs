using AutoMapper;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Repositories
{
    public class LanguageRepository(SchoolDbContext context, IMapper mapper)
        : IBasicRepository<Language>
    {
        public async Task<Language?> GetByIdAsync(Guid id)
        {
            var entity = await context.Languages
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
            
            return mapper.Map<Language>(entity);
        }

        public async Task<IEnumerable<Language>> GetAllAsync(IFilterDto? filter = null)
        {
            var entities = await context.Languages
                .AsNoTracking()
                .ToListAsync();
            
            return mapper.Map<IEnumerable<Language>>(entities);
        }

        public async Task<Language> AddAsync(Language language)
        {
            var entity = mapper.Map<LanguageEntity>(language);
            await context.Languages.AddAsync(entity);
            
            await context.SaveChangesAsync();
            return mapper.Map<Language>(entity);
        }

        public async Task<Language?> UpdateAsync(Guid id, Language language)
        {
            var existing = await context.Languages
                .FirstOrDefaultAsync(l => l.Id == id);
            
            if (existing == null) return null;

            context.Entry(existing)
                .CurrentValues.SetValues(mapper.Map<LanguageEntity>(language));
            
            await context.SaveChangesAsync();
            return mapper.Map<Language>(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await context.Languages
                .FirstOrDefaultAsync(l => l.Id == id);
            
            if (entity != null)
            {
                context.Languages.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
    
    //TODO: Add logger (ILogger or Serilog)
    //      Do the same in every repository and controller
    //      Check where and how to add other methods for sending email, authorization, google calendar and import/export data from files (excel, csv and etc...)
    //      Write possibility to add photo for users, students and employees
    //      Add filter method for students and employees
    //      Add validator
    //      Write possibility to check income of school (students payment amount)
    //      Connect this API to Frontend side
}