using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using CRMSystemApp.Models;
using CRMSystemApp.Services;

namespace CRMSystemApp.Views
{
    public partial class EmployeeDetailsCardView : UserControl
    {
        private readonly Employee _employee;

        public EmployeeDetailsCardView(Employee employee)
        {
            InitializeComponent();

            _employee = employee;

            ShowEmployeeInitials(employee.FullName);

            FullNameTextBlock.Text = employee.FullName;
            PositionTextBlock.Text = employee.Position;
            EmailTextBlock.Text = employee.Email;
            PhoneTextBlock.Text = employee.Phone;
            BirthDateTextBlock.Text = employee.BirthDate.ToString("dd.MM.yyyy");
            SalaryTextBlock.Text = $"{employee.Salary:C}";
        }

        #region BASE METHODS

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
            {
                e.Handled = true;
                SendMessage();
            }
        }

        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox tb) return;

            tb.Height = Double.NaN;
            tb.UpdateLayout();

            double currentHeight = tb.ActualHeight;
            double desiredHeight = tb.ExtentHeight + tb.Padding.Top + tb.Padding.Bottom;

            if (desiredHeight < tb.MinHeight)
                desiredHeight = tb.MinHeight;
            else if (desiredHeight > tb.MaxHeight)
                desiredHeight = tb.MaxHeight;

            double scale = desiredHeight / currentHeight;

            AnimateScale(scale);
        }

        #endregion

        #region ACTIONS

        private async void SendMessage()
        {
            var message = MessageTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show("Please enter a message before sending.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var subject = string.Empty;
                var body = string.Empty;

                var parts = message.Split(["??"], StringSplitOptions.None);

                if (parts.Length >= 2)
                {
                    subject = parts[0].Trim();
                    body = parts[1].Trim();
                }
                else body = message;

                var success = await EmailService.SendMessageAsync(_employee.Id, subject, body);

                if (success)
                {
                    MessageBox.Show("Message sent successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    MessageTextBox.Clear();
                }
                else MessageBox.Show("Failed to send the message.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while sending message: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        #endregion

        #region EMPLOYEE DETAILS CARD RENDERING

        private void ShowEmployeeInitials(string? fullName)
        {
            fullName = fullName?.Trim();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                CardInitialsTextBlock.Text = "";
                CardProfileImage.Visibility = Visibility.Visible;
                CardInitialsTextBlock.Visibility = Visibility.Visible;
                
                return;
            }

            var parts = fullName
                .Split([' '], StringSplitOptions.RemoveEmptyEntries)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToArray();

            string initials = "";

            if (parts.Length >= 1)
                initials += char.ToUpper(parts[0][0]);

            if (parts.Length >= 2)
                initials += char.ToUpper(parts[1][0]);

            CardInitialsTextBlock.Text = initials;

            CardInitialsTextBlock.Text = initials;
            CardInitialsTextBlock.Visibility = Visibility.Visible;
            CardProfileImage.Visibility = Visibility.Collapsed;
        }

        private void AnimateScale(double targetScaleY)
        {
            var animation = new DoubleAnimation
            {
                To = targetScaleY,
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut }
            };

            ContainerScale.BeginAnimation(ScaleTransform.ScaleYProperty, animation);
        }

        #endregion
    }
}