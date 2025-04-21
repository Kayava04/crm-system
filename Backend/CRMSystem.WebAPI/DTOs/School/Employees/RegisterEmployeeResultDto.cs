namespace CRMSystem.WebAPI.DTOs.School.Employees
{
    public class RegisterEmployeeResultDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public float Salary { get; set; }
    }
}