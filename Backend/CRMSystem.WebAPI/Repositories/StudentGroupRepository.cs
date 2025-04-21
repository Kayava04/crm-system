using AutoMapper;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Repositories
{
    public class StudentGroupRepository(SchoolDbContext context, IMapper mapper)
        : IBasicRepository<StudentGroup>
    {
        public async Task<StudentGroup?> GetByIdAsync(Guid id)
        {
            var entity = await context.StudentGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(sg => sg.Id == id);
            
            return mapper.Map<StudentGroup>(entity);
        }

        public async Task<IEnumerable<StudentGroup>> GetAllAsync(IFilterDto? filter = null)
        {
            var entities = await context.StudentGroups
                .AsNoTracking()
                .ToListAsync();
            
            return mapper.Map<IEnumerable<StudentGroup>>(entities);
        }

        public async Task<StudentGroup> AddAsync(StudentGroup model)
        {
            var entity = mapper.Map<StudentGroupEntity>(model);
            await context.StudentGroups.AddAsync(entity);
            
            await context.SaveChangesAsync();
            return mapper.Map<StudentGroup>(entity);
        }

        public async Task<StudentGroup?> UpdateAsync(Guid id, StudentGroup model)
        {
            var existing = await context.StudentGroups
                .FirstOrDefaultAsync(sg => sg.Id == id);
            
            if (existing == null) return null;

            context.Entry(existing)
                .CurrentValues.SetValues(mapper.Map<StudentGroupEntity>(model));
            
            await context.SaveChangesAsync();
            return mapper.Map<StudentGroup>(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await context.StudentGroups
                .FirstOrDefaultAsync(sg => sg.Id == id);
            
            if (entity != null)
            {
                context.StudentGroups.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}