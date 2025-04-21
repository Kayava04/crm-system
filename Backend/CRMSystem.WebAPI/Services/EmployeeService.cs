using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Employees;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class EmployeeService(IBasicRepository<Employee> employeeRepository, IMapper mapper)
    {
        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await employeeRepository.GetAllAsync();
            
            return mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto?> GetByIdAsync(Guid id)
        {
            var employee = await employeeRepository.GetByIdAsync(id);
            
            return employee is null ? null : mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
        {
            var employee = Employee.Create(Guid.NewGuid(), dto.PersonId, dto.Position, dto.Salary);
            var created = await employeeRepository.AddAsync(employee);
            
            return mapper.Map<EmployeeDto>(created);
        }

        public async Task<EmployeeDto?> UpdateAsync(Guid id, CreateEmployeeDto dto)
        {
            var employee = Employee.Create(id, dto.PersonId, dto.Position, dto.Salary);
            var updated = await employeeRepository.UpdateAsync(id, employee);
            
            return updated is null ? null : mapper.Map<EmployeeDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await employeeRepository.DeleteAsync(id);
        }
    }
}