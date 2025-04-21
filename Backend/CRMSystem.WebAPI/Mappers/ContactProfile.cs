using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Contacts;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Mappers
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<ContactEntity, Contact>().ConstructUsing(e =>
                Contact.Create(e.Id, e.PersonId, e.Phone, e.Email));
            
            CreateMap<Contact, ContactEntity>();
            CreateMap<Contact, ContactDto>();
            CreateMap<CreateContactDto, Contact>();
        }
    }
}