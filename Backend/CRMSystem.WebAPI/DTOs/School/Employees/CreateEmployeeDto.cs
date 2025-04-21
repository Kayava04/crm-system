namespace CRMSystem.WebAPI.DTOs.School.Employees
{
    public record CreateEmployeeDto(Guid PersonId, string Position, float Salary);
}