using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Students;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class StudentService(IBasicRepository<Student> studentRepository, IMapper mapper)
    {
        public async Task<IEnumerable<StudentDto>> GetAllAsync()
        {
            var students = await studentRepository.GetAllAsync();
            
            return mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto?> GetByIdAsync(Guid id)
        {
            var student = await studentRepository.GetByIdAsync(id);
            
            return student is null ? null : mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
        {
            var student = Student.Create(Guid.NewGuid(), dto.PersonId, dto.ParentId, dto.Grade, dto.IsPaid);
            var created = await studentRepository.AddAsync(student);
            
            return mapper.Map<StudentDto>(created);
        }

        public async Task<StudentDto?> UpdateAsync(Guid id, CreateStudentDto dto)
        {
            var student = Student.Create(id, dto.PersonId, dto.ParentId, dto.Grade, dto.IsPaid);
            var updated = await studentRepository.UpdateAsync(id, student);
            
            return updated is null ? null : mapper.Map<StudentDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await studentRepository.DeleteAsync(id);
        }
    }
}