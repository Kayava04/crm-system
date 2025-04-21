namespace CRMSystem.WebAPI.DTOs.School.Students
{
    public record CreateStudentDto(Guid PersonId, Guid? ParentId, int? Grade, bool IsPaid);
}