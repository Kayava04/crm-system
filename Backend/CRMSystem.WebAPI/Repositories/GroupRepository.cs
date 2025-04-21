using AutoMapper;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Repositories
{
    public class GroupRepository(SchoolDbContext context, IMapper mapper)
        : IBasicRepository<Group>
    {
        public async Task<Group?> GetByIdAsync(Guid id)
        {
            var entity = await context.Groups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);
            
            return mapper.Map<Group>(entity);
        }

        public async Task<IEnumerable<Group>> GetAllAsync(IFilterDto? filter = null)
        {
            var entities = await context.Groups
                .AsNoTracking()
                .ToListAsync();
            
            return mapper.Map<IEnumerable<Group>>(entities);
        }

        public async Task<Group> AddAsync(Group group)
        {
            var entity = mapper.Map<GroupEntity>(group);
            await context.Groups.AddAsync(entity);
            
            await context.SaveChangesAsync();
            return mapper.Map<Group>(entity);
        }

        public async Task<Group?> UpdateAsync(Guid id, Group group)
        {
            var existing = await context.Groups
                .FirstOrDefaultAsync(g => g.Id == id);
            
            if (existing == null) return null;

            context.Entry(existing)
                .CurrentValues.SetValues(mapper.Map<GroupEntity>(group));
            
            await context.SaveChangesAsync();
            return mapper.Map<Group>(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await context.Groups
                .FirstOrDefaultAsync(g => g.Id == id);
            
            if (entity != null)
            {
                context.Groups.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}