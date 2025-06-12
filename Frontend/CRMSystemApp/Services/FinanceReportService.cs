using CRMSystemApp.Models;

namespace CRMSystemApp.Services
{
    public static class FinanceReportService
    {
        public static async Task<ReportData> GetFinanceSummaryAsync()
        {
            return await ApiService.GetAsync<ReportData>("api/finance-report/summary");
        }
    }
}