using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.GroupLessonDays;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class GroupLessonDayService(IGroupLessonDayRepository repository, IMapper mapper)
    {
        public async Task<GroupLessonDayDto?> GetByIdAsync(Guid groupId, Guid lessonDayId)
        {
            var result = await repository.GetByIdAsync(groupId, lessonDayId);
            
            return result is null ? null : mapper.Map<GroupLessonDayDto>(result);
        }
        
        public async Task<IEnumerable<GroupLessonDayDto>> GetAllAsync()
        {
            var data = await repository.GetAllAsync();
            
            return mapper.Map<IEnumerable<GroupLessonDayDto>>(data);
        }

        public async Task<GroupLessonDayDto> CreateAsync(CreateGroupLessonDayDto dto)
        {
            var model = GroupLessonDay.Create(dto.GroupId, dto.LessonDayId);
            var created = await repository.AddAsync(model);
            
            return mapper.Map<GroupLessonDayDto>(created);
        }

        public async Task DeleteAsync(Guid groupId, Guid lessonDayId)
        {
            await repository.DeleteAsync(groupId, lessonDayId);
        }
    }
}