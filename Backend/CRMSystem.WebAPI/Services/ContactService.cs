using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Contacts;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class ContactService(IBasicRepository<Contact> contactRepository, IMapper mapper)
    {
        public async Task<IEnumerable<ContactDto>> GetAllAsync()
        {
            var contacts = await contactRepository.GetAllAsync();
            
            return mapper.Map<IEnumerable<ContactDto>>(contacts);
        }

        public async Task<ContactDto?> GetByIdAsync(Guid id)
        {
            var contact = await contactRepository.GetByIdAsync(id);
            
            return contact is null ? null : mapper.Map<ContactDto>(contact);
        }

        public async Task<ContactDto> CreateAsync(CreateContactDto dto)
        {
            var contact = Contact.Create(Guid.NewGuid(), dto.PersonId, dto.Phone, dto.Email);
            var created = await contactRepository.AddAsync(contact);
            
            return mapper.Map<ContactDto>(created);
        }

        public async Task<ContactDto?> UpdateAsync(Guid id, CreateContactDto dto)
        {
            var contact = Contact.Create(id, dto.PersonId, dto.Phone, dto.Email);
            var updated = await contactRepository.UpdateAsync(id, contact);
            
            return updated is null ? null : mapper.Map<ContactDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await contactRepository.DeleteAsync(id);
        }
    }
}