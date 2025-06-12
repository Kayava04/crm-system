using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Controls;
using System.Windows.Media;
using CRMSystemApp.Services;

namespace CRMSystemApp.Views
{
    public partial class ReportsView : UserControl
    {
        public SeriesCollection StudentIncomeSeries { get; set; }
        public SeriesCollection SchoolIncomeSeries { get; set; }
        public string[] StudentLabels { get; set; }
        public string[] SchoolLabels { get; set; }
        public Func<double, string> MoneyFormatter { get; set; }

        public ReportsView()
        {
            InitializeComponent();

            DataContext = this;
            
            LoadData();
        }

        #region FINANCE REPORT DATA VIEW RENDERING

        private async void LoadData()
        {
            var report = await FinanceReportService.GetFinanceSummaryAsync();

            var studentColor = (Color)ColorConverter.ConvertFromString("#3A7BD5");
            var schoolColor = (Color)ColorConverter.ConvertFromString("#22783E");

            // Diagram for Students \\
            StudentIncomeSeries =
            [
                new ColumnSeries
                {
                    Title = "Student Income\n      (Month)",
                    Values = new ChartValues<decimal>
                    {
                        report.StudentIncome.ExpectedIncome,
                        report.StudentIncome.ActualIncome
                    },
                    Fill = new SolidColorBrush(studentColor)
                }
            ];
            StudentLabels = ["Expected", "Actual"];

            // Diagram for Total School Net Profit \\
            SchoolIncomeSeries =
            [
                new ColumnSeries
                {
                    Title = "School Income\n     (Month)",
                    Values = new ChartValues<decimal>
                    {
                        report.TotalEmployeeSalaries,
                        report.NetProfit
                    },
                    Fill = new SolidColorBrush(schoolColor)
                }
            ];
            SchoolLabels = ["Employee Salaries", "Net Profit"];

            MoneyFormatter = value => $"{value:C0}";

            DataContext = null;
            DataContext = this;
        }

        #endregion
    }
}