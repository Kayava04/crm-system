using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Persons;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class PersonService(IBasicRepository<Person> personRepository, IMapper mapper)
    {
        public async Task<IEnumerable<PersonDto>> GetAllAsync()
        {
            var people = await personRepository.GetAllAsync();
            
            return mapper.Map<IEnumerable<PersonDto>>(people);
        }

        public async Task<PersonDto?> GetByIdAsync(Guid id)
        {
            var person = await personRepository.GetByIdAsync(id);
            
            return person is null ? null : mapper.Map<PersonDto>(person);
        }

        public async Task<PersonDto> CreateAsync(CreatePersonDto dto)
        {
            var person = Person.Create(Guid.NewGuid(), dto.FullName, dto.BirthDate);
            var created = await personRepository.AddAsync(person);
            
            return mapper.Map<PersonDto>(created);
        }

        public async Task<PersonDto?> UpdateAsync(Guid id, CreatePersonDto dto)
        {
            var person = Person.Create(id, dto.FullName, dto.BirthDate);
            var updated = await personRepository.UpdateAsync(id, person);
            
            return updated is null ? null : mapper.Map<PersonDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await personRepository.DeleteAsync(id);
        }
    }
}