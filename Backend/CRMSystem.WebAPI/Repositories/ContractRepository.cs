using AutoMapper;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Repositories
{
    public class ContractRepository(SchoolDbContext context, IMapper mapper)
        : IBasicRepository<Contract>
    {
        public async Task<Contract?> GetByIdAsync(Guid id)
        {
            var entity = await context.Contracts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            
            return mapper.Map<Contract>(entity);
        }

        public async Task<IEnumerable<Contract>> GetAllAsync(IFilterDto? filter = null)
        {
            var entities = await context.Contracts
                .AsNoTracking()
                .ToListAsync();
            
            return mapper.Map<IEnumerable<Contract>>(entities);
        }

        public async Task<Contract> AddAsync(Contract contract)
        {
            var entity = mapper.Map<ContractEntity>(contract);
            await context.Contracts.AddAsync(entity);
            
            await context.SaveChangesAsync();
            return mapper.Map<Contract>(entity);
        }

        public async Task<Contract?> UpdateAsync(Guid id, Contract contract)
        {
            var existing = await context.Contracts
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (existing == null) return null;

            context.Entry(existing)
                .CurrentValues.SetValues(mapper.Map<ContractEntity>(contract));
            
            await context.SaveChangesAsync();
            return mapper.Map<Contract>(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await context.Contracts
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (entity != null)
            {
                context.Contracts.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}