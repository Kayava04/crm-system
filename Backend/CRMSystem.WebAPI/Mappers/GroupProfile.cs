using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Groups;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Mappers
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupEntity, Group>().ConstructUsing(e =>
                Group.Create(e.Id, e.GroupName, e.LanguageId));
            
            CreateMap<Group, GroupEntity>();
            CreateMap<Group, GroupDto>();
            CreateMap<CreateGroupDto, Group>();
        }
    }
}