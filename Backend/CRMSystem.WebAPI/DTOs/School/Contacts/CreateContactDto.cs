namespace CRMSystem.WebAPI.DTOs.School.Contacts
{
    public record CreateContactDto(Guid PersonId, string Phone, string Email);
}