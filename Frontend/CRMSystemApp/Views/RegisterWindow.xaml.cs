using System.Windows;
using CRMSystemApp.Extensions;
using CRMSystemApp.Services;

namespace CRMSystemApp.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();

            RegisterBirthDateSelector.SelectedDate = DateTime.UtcNow;
        }

        #region BASE METHODS

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void CloseWindow_Click(object sender, RoutedEventArgs e) => Close();

        #endregion

        #region ACTIONS

        private void TurnBackButton_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();

            login.Show();
            Close();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(RegisterFullNameTextBox.Text) || string.IsNullOrWhiteSpace(RegisterEmailTextBox.Text)
                    || string.IsNullOrWhiteSpace(RegisterUsernameTextBox.Text) || string.IsNullOrWhiteSpace(RegisterPasswordTextBox.Password)
                    || string.IsNullOrWhiteSpace(RegisterConfirmPasswordTextBox.Password))
                {
                    MessageBox.Show("Please enter all fields below.", "Incorrect Data", MessageBoxButton.OK);
                    return;
                }

                if (RegisterBirthDateSelector.SelectedDate >= DateTime.Today)
                {
                    MessageBox.Show("'Birth Date' is incorrect.", "Incorrect Data", MessageBoxButton.OK);
                    return;
                }

                var success = await AuthService.RegisterAsync(
                    RegisterFullNameTextBox.Text,
                    RegisterBirthDateSelector.SelectedDate.ToUtcDate(),
                    RegisterEmailTextBox.Text,
                    RegisterUsernameTextBox.Text,
                    RegisterPasswordTextBox.Password,
                    RegisterConfirmPasswordTextBox.Password);
                
                if (success)
                {
                    var login = new LoginWindow();
                    Application.Current.MainWindow = login;
                    
                    MessageBox.Show("Registration successfully!", "Success", MessageBoxButton.OK);

                    login.Show();
                    Close();
                }
                else MessageBox.Show("Registration failed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Please check your data: {ex.Message}", "Incorrect Data", MessageBoxButton.OK);
            }
        }

        #endregion
    }
}