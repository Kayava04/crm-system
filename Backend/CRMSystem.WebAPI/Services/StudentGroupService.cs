using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.StudentGroups;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class StudentGroupService(IBasicRepository<StudentGroup> repository, IMapper mapper)
    {
        public async Task<IEnumerable<StudentGroupDto>> GetAllAsync()
        {
            var data = await repository.GetAllAsync();
            
            return mapper.Map<IEnumerable<StudentGroupDto>>(data);
        }

        public async Task<StudentGroupDto?> GetByIdAsync(Guid id)
        {
            var item = await repository.GetByIdAsync(id);
            
            return item is null ? null : mapper.Map<StudentGroupDto>(item);
        }

        public async Task<StudentGroupDto> CreateAsync(CreateStudentGroupDto dto)
        {
            var model = StudentGroup.Create(Guid.NewGuid(), dto.StudentId, dto.GroupId, dto.Level, dto.PairNumber);
            var created = await repository.AddAsync(model);
            
            return mapper.Map<StudentGroupDto>(created);
        }

        public async Task<StudentGroupDto?> UpdateAsync(Guid id, CreateStudentGroupDto dto)
        {
            var model = StudentGroup.Create(id, dto.StudentId, dto.GroupId, dto.Level, dto.PairNumber);
            var updated = await repository.UpdateAsync(id, model);
            
            return updated is null ? null : mapper.Map<StudentGroupDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await repository.DeleteAsync(id);
        }
    }
}