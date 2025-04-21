namespace CRMSystem.WebAPI.DTOs.FinanceReport
{
    public class TotalSchoolIncomeReportDto
    {
        public StudentIncomeReportDto StudentIncome { get; set; } = null!;
        public int TotalEmployees { get; set; }
        public decimal TotalEmployeeSalaries { get; set; }
        public decimal NetProfit { get; set; }
    }
}