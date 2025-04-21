namespace CRMSystem.WebAPI.DTOs.School.Employees
{
    public record EmployeeDto(Guid Id, Guid PersonId, string Position, float Salary);
}