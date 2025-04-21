using CRMSystem.WebAPI.Interfaces;

namespace CRMSystem.WebAPI.DTOs.School.Employees
{
    public class EmployeeFilterDto : IFilterDto
    {
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Position { get; set; }
        public float? MinSalary { get; set; }
        public float? MaxSalary { get; set; }
    }
}