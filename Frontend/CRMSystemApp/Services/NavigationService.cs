using System.Windows.Controls;
using CRMSystemApp.Views;

namespace CRMSystemApp.Services
{
    public static class NavigationService
    {
        public static ContentControl MainContentControl { get; set; }

        public static async void NavigateTo(UserControl view)
        {
            if (await AuthGuardService.ValidateAuthenticationAsync())
                MainContentControl.Content = view;
            else
                System.Windows.MessageBox.Show("Session expired. Please login again.");
        }

        // Shortcuts \\
        public static void Home() => NavigateTo(new HomeView());
        public static void Students() => NavigateTo(new StudentsView());
        public static void Employees() => NavigateTo(new EmployeesView());
        public static void Reports() => NavigateTo(new ReportsView());
        public static void Email() => NavigateTo(new EmailView());
        public static void Calendar() => NavigateTo(new CalendarView());
        public static void Salary() => NavigateTo(new SalaryView());
        public static void Schedule() => NavigateTo(new ScheduleView());
        public static void Tasks() => NavigateTo(new TaskManagerView());
        public static void About() => NavigateTo(new AboutView());
    }
}