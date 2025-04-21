using CRMSystem.WebAPI.DTOs.FinanceReport;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class FinanceReportService(
        IBasicRepository<Student> studentRepository,
        IBasicRepository<Contract> contractRepository,
        IBasicRepository<Employee> employeeRepository)
    {
        public async Task<TotalSchoolIncomeReportDto> GenerateReportAsync()
        {
            var students = await studentRepository.GetAllAsync();
            var contracts = await contractRepository.GetAllAsync();
            var employees = await employeeRepository.GetAllAsync();
            
            var totalStudents = students.Count();
            var expectedIncome = contracts.Sum(c => c.PaymentAmount);
            var actualIncome = (
                from s in students
                join c in contracts on s.Id equals c.StudentId
                where s.IsPaid
                select c.PaymentAmount).Sum();

            var studentReport = new StudentIncomeReportDto
            {
                TotalStudents = totalStudents,
                ExpectedIncome = expectedIncome,
                ActualIncome = actualIncome
            };
            
            var totalEmployees = employees.Count();
            var totalSalaries = employees.Sum(e => (decimal)e.Salary);
            var netProfit = actualIncome - totalSalaries;

            return new TotalSchoolIncomeReportDto
            {
                StudentIncome = studentReport,
                TotalEmployees = totalEmployees,
                TotalEmployeeSalaries = totalSalaries,
                NetProfit = netProfit
            };
        }
    }
}