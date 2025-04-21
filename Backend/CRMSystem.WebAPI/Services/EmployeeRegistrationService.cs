using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Employees;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class EmployeeRegistrationService(
        IBasicRepository<Person> personRepository,
        IBasicRepository<Employee> employeeRepository,
        IBasicRepository<Contact> contactRepository,
        IMapper mapper)
    {
        public async Task<RegisterEmployeeResultDto> CreateEmployeeAsync(RegisterEmployeeDto dto)
        {
            var person = Person.Create(Guid.NewGuid(), dto.FullName, dto.BirthDate);
            var createdPerson = await personRepository.AddAsync(person);
            
            var employee = Employee.Create(Guid.NewGuid(), createdPerson.Id, dto.Position, dto.Salary);
            var createdEmployee = await employeeRepository.AddAsync(employee);
            
            var contact = Contact.Create(Guid.NewGuid(), createdPerson.Id, dto.Phone, dto.Email);
            var createdContact = await contactRepository.AddAsync(contact);
            
            return mapper.Map<RegisterEmployeeResultDto>((createdEmployee, createdPerson, createdContact));
        }
        
        public async Task<RegisterEmployeeResultDto?> GetEmployeeByIdAsync(Guid employeeId)
        {
            var employee = await employeeRepository.GetByIdAsync(employeeId);
            if (employee == null) return null;

            var person = await personRepository.GetByIdAsync(employee.PersonId);
            var contact = (await contactRepository.GetAllAsync()).FirstOrDefault(c => c.PersonId == employee.PersonId);

            if (person == null || contact == null) return null;
            
            return mapper.Map<RegisterEmployeeResultDto>((employee, person, contact));
        }
        
        public async Task<IEnumerable<RegisterEmployeeResultDto>> GetAllEmployeesAsync(EmployeeFilterDto? filter = null)
        {
            var employees = await employeeRepository.GetAllAsync(filter);
            var results = new List<RegisterEmployeeResultDto>();

            foreach (var employee in employees)
            {
                var person = await personRepository.GetByIdAsync(employee.PersonId);
                var contact = (await contactRepository.GetAllAsync()).FirstOrDefault(c => c.PersonId == employee.PersonId);

                if (person == null || contact == null) continue;
                var result = mapper.Map<RegisterEmployeeResultDto>((employee, person, contact));
                results.Add(result);
            }

            return results;
        }
        
        public async Task<RegisterEmployeeResultDto?> UpdateEmployeeAsync(Guid id, RegisterEmployeeDto dto)
        {
            var employee = await employeeRepository.GetByIdAsync(id);
            if (employee == null) return null;

            var person = await personRepository.GetByIdAsync(employee.PersonId);
            if (person == null) return null;

            var contact = (await contactRepository.GetAllAsync()).FirstOrDefault(c => c.PersonId == person.Id);
            
            person.Update(dto.FullName, dto.BirthDate);
            employee.Update(dto.Position, dto.Salary);

            if (contact != null)
            {
                contact.Update(dto.Phone, dto.Email);
                await contactRepository.UpdateAsync(contact.Id, contact);
            }

            await personRepository.UpdateAsync(person.Id, person);
            await employeeRepository.UpdateAsync(id, employee);

            var result = mapper.Map<RegisterEmployeeResultDto>(
                (employee, person, contact ?? Contact.Create(Guid.NewGuid(), person.Id, dto.Phone, dto.Email))
            );
            return result;
        }
        
        public async Task DeleteEmployeeAsync(Guid id)
        {
            var employee = await employeeRepository.GetByIdAsync(id);
            if (employee == null) return;

            var person = await personRepository.GetByIdAsync(employee.PersonId);
            if (person == null) return;

            var contact = (await contactRepository.GetAllAsync()).FirstOrDefault(c => c.PersonId == person.Id);
            if (contact != null) await contactRepository.DeleteAsync(contact.Id);

            await employeeRepository.DeleteAsync(id);
            await personRepository.DeleteAsync(person.Id);
        }
    }
}