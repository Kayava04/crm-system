using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Groups;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class GroupService(IBasicRepository<Group> groupRepository, IMapper mapper)
    {
        public async Task<IEnumerable<GroupDto>> GetAllAsync()
        {
            var groups = await groupRepository.GetAllAsync();
            
            return mapper.Map<IEnumerable<GroupDto>>(groups);
        }

        public async Task<GroupDto?> GetByIdAsync(Guid id)
        {
            var group = await groupRepository.GetByIdAsync(id);
            
            return group is null ? null : mapper.Map<GroupDto>(group);
        }

        public async Task<GroupDto> CreateAsync(CreateGroupDto dto)
        {
            var group = Group.Create(Guid.NewGuid(), dto.GroupName, dto.LanguageId);
            var created = await groupRepository.AddAsync(group);
            
            return mapper.Map<GroupDto>(created);
        }

        public async Task<GroupDto?> UpdateAsync(Guid id, CreateGroupDto dto)
        {
            var group = Group.Create(id, dto.GroupName, dto.LanguageId);
            var updated = await groupRepository.UpdateAsync(id, group);
            
            return updated is null ? null : mapper.Map<GroupDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await groupRepository.DeleteAsync(id);
        }
    }
}