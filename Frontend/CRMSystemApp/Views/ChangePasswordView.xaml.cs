using System.Windows;
using System.Windows.Controls;
using CRMSystemApp.DTOs.User;
using CRMSystemApp.Services;

namespace CRMSystemApp.Views
{
    public partial class ChangePasswordView : UserControl
    {
        public ChangePasswordView()
        {
            InitializeComponent();
        }

        #region ACTIONS

        private void TurnBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is AccountWindow window)
                window.NavigateInsideAccount(new AccountView());
        }

        private async void ChangePasswordConfirm_Click(object sender, RoutedEventArgs e)
        {
            var passwordDto = new UpdatePasswordDto
            {
                OldPassword = OldPasswordTextBox.Password,
                NewPassword = NewPasswordTextBox.Password,
                ConfirmNewPassword = ConfirmPasswordTextBox.Password
            };

            try
            {
                var success = await UserService.UpdatePasswordAsync(passwordDto);

                if (success)
                    MessageBox.Show("Password changed successfully!", "Success", MessageBoxButton.OK);
                else
                    MessageBox.Show("Failed to change password.", "Error", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while changing password: {ex.Message}", "Error", MessageBoxButton.OK);
            }

            OldPasswordTextBox.Clear();
            NewPasswordTextBox.Clear();
            ConfirmPasswordTextBox.Clear();
        }

        #endregion
    }
}