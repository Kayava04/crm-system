using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Employees;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Mappers
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeEntity, Employee>()
                .ConstructUsing(e => Employee.Create(e.Id, e.PersonId, e.Position, e.Salary));
            
            CreateMap<Employee, EmployeeEntity>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CreateEmployeeDto, Employee>();
            
            CreateMap<(Employee employee, Person person, Contact contact), RegisterEmployeeResultDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.employee.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.person.FullName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.person.BirthDate))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.contact.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.contact.Email))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.employee.Position))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.employee.Salary));
            
            CreateMap<RegisterEmployeeResultDto, RegisterEmployeeDto>();
        }
    }
}