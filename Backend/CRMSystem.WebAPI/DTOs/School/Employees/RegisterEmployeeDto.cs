namespace CRMSystem.WebAPI.DTOs.School.Employees
{
    public class RegisterEmployeeDto
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public float Salary { get; set; }
    }
}