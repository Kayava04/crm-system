using System.Windows;
using CRMSystemApp.Services;

namespace CRMSystemApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        #region BASE METHODS

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void CloseWindow_Click(object sender, RoutedEventArgs e) => Close();

        #endregion

        #region ACTIONS

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LoginUsernameTextBox.Text) || string.IsNullOrWhiteSpace(LoginPasswordTextBox.Password))
                {
                    MessageBox.Show("Please enter 'Username' and 'Password'.", "Incorrect Data", MessageBoxButton.OK);
                    return;
                }

                var success = await AuthService.LoginAsync(LoginUsernameTextBox.Text, LoginPasswordTextBox.Password);
                
                if (success)
                {
                    var main = new MainWindow();
                    Application.Current.MainWindow = main;
                    
                    main.Show();
                    Close();
                }
                else MessageBox.Show("Login failed.");
            }
            catch (Exception)
            {
                MessageBox.Show("'Username' or 'Password' is incorrect.", "Incorrect Data", MessageBoxButton.OK);
            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            var register = new RegisterWindow();
            
            register.Show();
            Close();
        }

        #endregion
    }
}