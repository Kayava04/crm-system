using AutoMapper;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Repositories
{
    public class LessonDayRepository(SchoolDbContext context, IMapper mapper)
        : IBasicRepository<LessonDay>
    {
        public async Task<LessonDay?> GetByIdAsync(Guid id)
        {
            var entity = await context.LessonDays
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
            
            return mapper.Map<LessonDay>(entity);
        }

        public async Task<IEnumerable<LessonDay>> GetAllAsync(IFilterDto? filter = null)
        {
            var entities = await context.LessonDays
                .AsNoTracking()
                .ToListAsync();
            
            return mapper.Map<IEnumerable<LessonDay>>(entities);
        }

        public async Task<LessonDay> AddAsync(LessonDay lessonDay)
        {
            var entity = mapper.Map<LessonDayEntity>(lessonDay);
            await context.LessonDays.AddAsync(entity);
            
            await context.SaveChangesAsync();
            return mapper.Map<LessonDay>(entity);
        }

        public async Task<LessonDay?> UpdateAsync(Guid id, LessonDay lessonDay)
        {
            var existing = await context.LessonDays
                .FirstOrDefaultAsync(l => l.Id == id);
            
            if (existing == null) return null;

            context.Entry(existing)
                .CurrentValues.SetValues(mapper.Map<LessonDayEntity>(lessonDay));
            
            await context.SaveChangesAsync();
            return mapper.Map<LessonDay>(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await context.LessonDays
                .FirstOrDefaultAsync(l => l.Id == id);
            
            if (entity != null)
            {
                context.LessonDays.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}