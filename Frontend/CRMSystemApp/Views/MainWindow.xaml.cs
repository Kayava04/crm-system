using System.Windows;
using System.Windows.Input;
using CRMSystemApp.Services;

namespace CRMSystemApp.Views
{
    public partial class MainWindow : Window
    {
        private bool isMaximized = false;

        public MainWindow()
        {
            InitializeComponent();

            NavigationService.MainContentControl = MainContent;
            NavigationService.Home();
        }

        #region BASE METHODS

        private void TopPanelWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                ToggleMaximize_Click(sender, e);
            else if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void MaximizeWindow_Click(object sender, RoutedEventArgs e) => ToggleMaximize_Click(sender, e);

        private void CloseWindow_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        #endregion

        #region FUNCTIONAL SECTION BUTTONS

        private void OpenHomeView_Click(object sender, RoutedEventArgs e) => NavigationService.Home();

        private void SwitchTheme_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenProfile_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = new AccountWindow();
            profileWindow.Show();
        }

        #endregion

        #region TAB BUTTONS

        private void OpenStudentsView_Click(object sender, RoutedEventArgs e) => NavigationService.Students();
        private void OpenEmployeesView_Click(object sender, RoutedEventArgs e) => NavigationService.Employees();
        private void OpenScheduleView_Click(object sender, RoutedEventArgs e) => NavigationService.Schedule();
        private void OpenEmailView_Click(object sender, RoutedEventArgs e) => NavigationService.Email();
        private void OpenReportsView_Click(object sender, RoutedEventArgs e) => NavigationService.Reports();
        private void OpenSalaryView_Click(object sender, RoutedEventArgs e) => NavigationService.Salary();
        private void OpenCalendarView_Click(object sender, RoutedEventArgs e) => NavigationService.Calendar();
        private void OpenTaskManagerView_Click(object sender, RoutedEventArgs e) => NavigationService.Tasks();
        private void OpenAboutView_Click(object sender, RoutedEventArgs e) => NavigationService.About();

        #endregion

        #region SWITCH THEME

        private void SwitchThemeToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            //ApplyTheme("DarkTheme.xaml");
        }

        private void SwitchThemeToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            //ApplyTheme("LightTheme.xaml");
        }

        private void ApplyTheme(string path)
        {
            var uri = new Uri($"Themes/{path}", UriKind.Relative);
            var newTheme = Application.LoadComponent(uri) as ResourceDictionary;

            if (newTheme == null) return;

            var dictionaries = Application.Current.Resources.MergedDictionaries;

            var oldTheme = dictionaries.FirstOrDefault(t => t.Source != null && t.Source.OriginalString.Contains("Themes/"));
            if (oldTheme != null)
                dictionaries.Remove(oldTheme);

            dictionaries.Add(newTheme);
        }

        #endregion

        #region WINDOW RENDERING

        private void ToggleMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (!isMaximized)
            {
                Left = SystemParameters.WorkArea.Left;
                Top = SystemParameters.WorkArea.Top;
                Width = SystemParameters.WorkArea.Width;
                Height = SystemParameters.WorkArea.Height;

                MainWindowBorder.CornerRadius = new CornerRadius(0);
                TabButtonsBorder.CornerRadius = new CornerRadius(0);

                CloseButton.Style = (Style)FindResource("CloseFullscreenStyle");

                if (MainContent.Content is EmployeesView employeesView)
                    employeesView.EmployeeFilterTextBox.Width = 300;

                isMaximized = true;
            }
            else
            {
                Window_Loaded(sender, e);

                MainWindowBorder.CornerRadius = new CornerRadius(20, 20, 20, 25);
                TabButtonsBorder.CornerRadius = new CornerRadius(0, 0, 0, 20);

                CloseButton.Style = (Style)FindResource("CloseStyle");

                if (MainContent.Content is EmployeesView employeesView)
                    employeesView.EmployeeFilterTextBox.Width = 250;

                isMaximized = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var screenWidth = SystemParameters.WorkArea.Width;
            var screenHeight = SystemParameters.WorkArea.Height;

            Width = screenWidth * 0.75;
            Height = screenHeight * 0.75;

            MinWidth = 1024;
            MinHeight = 720;

            Left = (screenWidth - Width) / 2;
            Top = (screenHeight - Height) / 2;
        }

        #endregion
    }
}