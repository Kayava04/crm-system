using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.GroupLessonDays;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Mappers
{
    public class GroupLessonDayProfile : Profile
    {
        public GroupLessonDayProfile()
        {
            CreateMap<GroupLessonDayEntity, GroupLessonDay>().ConstructUsing(e =>
                GroupLessonDay.Create(e.GroupId, e.LessonDayId));
            
            CreateMap<GroupLessonDay, GroupLessonDayEntity>();
            CreateMap<GroupLessonDay, GroupLessonDayDto>();
            CreateMap<CreateGroupLessonDayDto, GroupLessonDay>();
        }
    }
}