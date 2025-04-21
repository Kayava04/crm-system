using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.LessonDays;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Mappers
{
    public class LessonDayProfile : Profile
    {
        public LessonDayProfile()
        {
            CreateMap<LessonDayEntity, LessonDay>().ConstructUsing(e =>
                LessonDay.Create(e.Id, e.DayOfWeek));
            
            CreateMap<LessonDay, LessonDayEntity>();
            CreateMap<LessonDay, LessonDayDto>();
            CreateMap<CreateLessonDayDto, LessonDay>();
        }
    }
}