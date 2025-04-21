namespace CRMSystem.WebAPI.DTOs.FinanceReport
{
    public class StudentIncomeReportDto
    {
        public int TotalStudents { get; set; }
        public decimal ExpectedIncome { get; set; }
        public decimal ActualIncome { get; set; }
    }
}