using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.StudentGroups;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Mappers
{
    public class StudentGroupProfile : Profile
    {
        public StudentGroupProfile()
        {
            CreateMap<StudentGroupEntity, StudentGroup>().ConstructUsing(e =>
                StudentGroup.Create(e.Id, e.StudentId, e.GroupId, e.Level, e.PairNumber));
            
            CreateMap<StudentGroup, StudentGroupEntity>();
            CreateMap<StudentGroup, StudentGroupDto>();
            CreateMap<CreateStudentGroupDto, StudentGroup>();
        }
    }
}