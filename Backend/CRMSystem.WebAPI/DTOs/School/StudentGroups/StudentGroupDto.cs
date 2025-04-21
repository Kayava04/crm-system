namespace CRMSystem.WebAPI.DTOs.School.StudentGroups
{
    public record StudentGroupDto(Guid Id, Guid StudentId, Guid GroupId, string Level, int PairNumber);
}