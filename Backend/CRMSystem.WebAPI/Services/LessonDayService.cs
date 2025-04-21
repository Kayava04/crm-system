using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.LessonDays;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class LessonDayService(IBasicRepository<LessonDay> repository, IMapper mapper)
    {
        public async Task<IEnumerable<LessonDayDto>> GetAllAsync()
        {
            var result = await repository.GetAllAsync();
            
            return mapper.Map<IEnumerable<LessonDayDto>>(result);
        }

        public async Task<LessonDayDto?> GetByIdAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id);
            
            return entity is null ? null : mapper.Map<LessonDayDto>(entity);
        }

        public async Task<LessonDayDto> CreateAsync(CreateLessonDayDto dto)
        {
            var model = LessonDay.Create(Guid.NewGuid(), dto.DayOfWeek);
            var created = await repository.AddAsync(model);
            
            return mapper.Map<LessonDayDto>(created);
        }

        public async Task<LessonDayDto?> UpdateAsync(Guid id, CreateLessonDayDto dto)
        {
            var model = LessonDay.Create(id, dto.DayOfWeek);
            var updated = await repository.UpdateAsync(id, model);
            
            return updated is null ? null : mapper.Map<LessonDayDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await repository.DeleteAsync(id);
        }
    }
}