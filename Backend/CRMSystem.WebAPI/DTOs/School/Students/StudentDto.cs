namespace CRMSystem.WebAPI.DTOs.School.Students
{
    public record StudentDto(Guid Id, Guid PersonId, Guid? ParentId, int? Grade, bool IsPaid);
}