using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Persons;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Mappers
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<PersonEntity, Person>().ConstructUsing(e =>
                Person.Create(e.Id, e.FullName, e.BirthDate));
            
            CreateMap<Person, PersonEntity>();
            CreateMap<Person, PersonDto>();
            CreateMap<CreatePersonDto, Person>();
        }
    }
}