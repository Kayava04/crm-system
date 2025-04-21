using CRMSystem.WebAPI.Interfaces;

namespace CRMSystem.WebAPI.DTOs.School.Students
{
    public class StudentFilterDto : IFilterDto
    {
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }

        public int? Grade { get; set; }
        
        public string? ParentFullName { get; set; }

        public string? LanguageName { get; set; }
        public string? Level { get; set; }
        public string? GroupName { get; set; }
        public string? LessonDay { get; set; }
        public int? PairNumber { get; set; }

        public string? ContractNumber { get; set; }
        public bool? IsPaid { get; set; }
    }
}