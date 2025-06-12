namespace CRMSystemApp.DTOs.Employee
{
    public class EmployeeFilter
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