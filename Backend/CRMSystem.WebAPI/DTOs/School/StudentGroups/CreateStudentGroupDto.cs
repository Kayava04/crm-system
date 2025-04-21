namespace CRMSystem.WebAPI.DTOs.School.StudentGroups
{
    public record CreateStudentGroupDto(Guid StudentId, Guid GroupId, string Level, int PairNumber);
}