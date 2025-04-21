using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Interfaces
{
    public interface IGroupLessonDayRepository
    {
        Task<GroupLessonDay?> GetByIdAsync(Guid groupId, Guid lessonDayId);
        Task<List<LessonDay>> GetLessonDaysByGroupIdAsync(Guid groupId);
        Task<IEnumerable<GroupLessonDay>> GetAllAsync();
        Task<GroupLessonDay> AddAsync(GroupLessonDay entity);
        Task DeleteAsync(Guid groupId, Guid lessonDayId);
    }
}