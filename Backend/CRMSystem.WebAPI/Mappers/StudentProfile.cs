using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Students;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Mappers
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<StudentEntity, Student>()
                .ConstructUsing(e => Student.Create(e.Id, e.PersonId, e.ParentId, e.Grade, e.IsPaid));

            CreateMap<Student, StudentEntity>()
                .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => src.IsPaid));
            
            CreateMap<Student, StudentDto>();
            
            CreateMap<CreateStudentDto, Student>()
                .ConstructUsing(dto => Student.Create(Guid.NewGuid(), dto.PersonId, dto.ParentId, dto.Grade, false));
            
            CreateMap<(Student student, Person person, Contact contact, Contract contract, Group group, Language language, List<LessonDay> lessonDays, StudentGroup studentGroup, Person? parent), RegisterStudentDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.person.FullName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.person.BirthDate))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.contact.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.contact.Email))
                .ForMember(dest => dest.ParentFullName, opt => opt.MapFrom(src => src.parent != null ? src.parent.FullName : null))
                .ForMember(dest => dest.IsParentRegister, opt => opt.MapFrom(src => src.parent != null))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.student.Grade))
                .ForMember(dest => dest.SignDate, opt => opt.MapFrom(src => src.contract.SignDate))
                .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.contract.PaymentAmount))
                .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.language.Name))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.group.GroupName))
                .ForMember(dest => dest.LessonDays, opt => opt.MapFrom(src =>
                    string.Join(", ", src.lessonDays.Select(ld => ld.DayOfWeek.ToString()))
                ))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.studentGroup.Level))
                .ForMember(dest => dest.PairNumber, opt => opt.MapFrom(src => src.studentGroup.PairNumber))
                .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => src.student.IsPaid));
            
            CreateMap<(Student student, Person person, Contact contact, Contract contract, Group group, Language language, List<LessonDay> lessonDays, StudentGroup studentGroup, Person? parent), RegisterStudentResultDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.student.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.person.FullName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.person.BirthDate))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.contact.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.contact.Email))
                .ForMember(dest => dest.ParentFullName, opt => opt.MapFrom(src => src.parent.FullName))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.student.Grade))
                .ForMember(dest => dest.ContractNumber, opt => opt.MapFrom(src => src.contract.ContractNumber))
                .ForMember(dest => dest.SignDate, opt => opt.MapFrom(src => src.contract.SignDate))
                .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.contract.PaymentAmount))
                .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.language.Name))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.group.GroupName))
                .ForMember(dest => dest.LessonDays, opt => opt.MapFrom(src =>
                    string.Join(", ", src.lessonDays.Select(ld => ld.DayOfWeek.ToString()))
                ))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.studentGroup.Level))
                .ForMember(dest => dest.PairNumber, opt => opt.MapFrom(src => src.studentGroup.PairNumber))
                .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => src.student.IsPaid));
            
            CreateMap<RegisterStudentDto, RegisterStudentResultDto>();
            CreateMap<RegisterStudentResultDto, RegisterStudentDto>();

        }
    }
}