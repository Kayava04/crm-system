using AutoMapper;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Repositories
{
    public class GroupLessonDayRepository(SchoolDbContext context, IMapper mapper)
        : IGroupLessonDayRepository
    {
        public async Task<GroupLessonDay?> GetByIdAsync(Guid groupId, Guid lessonDayId)
        {
            var entity = await context.GroupLessonDays
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.GroupId == groupId && e.LessonDayId == lessonDayId);

            return mapper.Map<GroupLessonDay>(entity);
        }
        
        public async Task<List<LessonDay>> GetLessonDaysByGroupIdAsync(Guid groupId)
        {
            var groupLessonDays = await context.GroupLessonDays
                .Where(gld => gld.GroupId == groupId)
                    .Include(gld => gld.LessonDay)
                        .Select(gld => gld.LessonDay)
                            .ToListAsync();

            return mapper.Map<List<LessonDay>>(groupLessonDays);
        }
        
        public async Task<IEnumerable<GroupLessonDay>> GetAllAsync()
        {
            var entities = await context.GroupLessonDays
                .AsNoTracking()
                .ToListAsync();
            
            return mapper.Map<IEnumerable<GroupLessonDay>>(entities);
        }

        public async Task<GroupLessonDay> AddAsync(GroupLessonDay model)
        {
            var entity = mapper.Map<GroupLessonDayEntity>(model);
            await context.GroupLessonDays.AddAsync(entity);
            
            await context.SaveChangesAsync();
            return mapper.Map<GroupLessonDay>(entity);
        }

        public async Task DeleteAsync(Guid groupId, Guid lessonDayId)
        {
            var entity = await context.GroupLessonDays
                .FirstOrDefaultAsync(e => e.GroupId == groupId && e.LessonDayId == lessonDayId);

            if (entity != null)
            {
                context.GroupLessonDays.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}