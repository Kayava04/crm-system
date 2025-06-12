using CRMSystemApp.DTOs.FinanceReport;

namespace CRMSystemApp.Models
{
    public class ReportData
    {
        public StudentIncomeReportDto StudentIncome { get; set; } = new StudentIncomeReportDto();
        public int TotalEmployees { get; set; }
        public decimal TotalEmployeeSalaries { get; set; }
        public decimal NetProfit { get; set; }
    }
}