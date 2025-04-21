using AutoMapper;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.DTOs.School.Employees;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Repositories
{
    public class EmployeeRepository(SchoolDbContext context, IMapper mapper)
        : IBasicRepository<Employee>
    {
        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            var entity = await context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
            
            return mapper.Map<Employee>(entity);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(IFilterDto? filter = null)
        {
            var query = context.Employees
                .Include(e => e.Person)
                    .ThenInclude(p => p.Contacts)
                        .AsNoTracking();

            var employeeFilter = filter as EmployeeFilterDto;
            
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(employeeFilter.FullName))
                    query = query.Where(e => e.Person.FullName
                        .Contains(employeeFilter.FullName));

                if (employeeFilter.BirthDate.HasValue)
                    query = query.Where(e => e.Person.BirthDate.Date == employeeFilter.BirthDate.Value.Date);

                if (!string.IsNullOrWhiteSpace(employeeFilter.Phone))
                    query = query.Where(e => e.Person.Contacts
                        .Any(c => c.Phone == employeeFilter.Phone));

                if (!string.IsNullOrWhiteSpace(employeeFilter.Email))
                    query = query.Where(e => e.Person.Contacts
                        .Any(c => c.Email == employeeFilter.Email));

                if (!string.IsNullOrWhiteSpace(employeeFilter.Position))
                    query = query.Where(e => e.Position
                        .Contains(employeeFilter.Position));
                
                if (employeeFilter.MinSalary.HasValue)
                    query = query.Where(e => e.Salary >= employeeFilter.MinSalary.Value);

                if (employeeFilter.MaxSalary.HasValue)
                    query = query.Where(e => e.Salary <= employeeFilter.MaxSalary.Value);
            }
                
            return mapper.Map<IEnumerable<Employee>>(await query.ToListAsync());
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            var entity = mapper.Map<EmployeeEntity>(employee);
            await context.Employees.AddAsync(entity);
            
            await context.SaveChangesAsync();
            return mapper.Map<Employee>(entity);
        }

        public async Task<Employee?> UpdateAsync(Guid id, Employee employee)
        {
            var entity = await context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (entity == null) return null;

            mapper.Map(employee, entity);
            
            await context.SaveChangesAsync();
            return mapper.Map<Employee>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (entity != null)
            {
                context.Employees.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}