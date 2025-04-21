namespace CRMSystem.WebAPI.DTOs.School.Contacts
{
    public record ContactDto(Guid Id, Guid PersonId, string Phone, string Email);
}