using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRMSystemApp.Models;
using CRMSystemApp.Services;

namespace CRMSystemApp.Views
{
    public partial class EmailView : UserControl
    {
        private List<Message> AllEmails = [];
        private List<Message> Emails = [];
        private Message? SelectedEmail;

        public EmailView()
        {
            InitializeComponent();

            _ = LoadEmails();
            EmailDataGrid.PreviewKeyDown += EmailDataGrid_PreviewKeyDown;
        }

        #region DATAGRID RENDERING

        private async Task LoadEmails()
        {
            try
            {
                var messages = await EmailService.GetAllMessagesAsync();
                var employees = (await EmployeeService.GetAllEmployeesAsync()).ToList();
                var students = (await StudentService.GetAllStudentsAsync()).ToList();

                AllEmails = messages
                    .OrderByDescending(m => m.SentAt)
                    .Select((m, index) =>
                {
                    string receiverName = "Unknown";

                    var employee = employees.FirstOrDefault(emp => emp.Id == Guid.Parse(m.ReceiverId));
                    
                    if (employee != null)
                        receiverName = employee.FullName;
                    else
                    {
                        var student = students.FirstOrDefault(st => st.Id == Guid.Parse(m.ReceiverId));
                        
                        if (student != null)
                            receiverName = student.FullName;
                    }

                    return new Message
                    {
                        RowNumber = index + 1,
                        Id = m.Id,
                        Subject = m.Subject,
                        Body = m.Body,
                        SentAt = m.SentAt,
                        Email = m.Email,
                        ReceiverName = receiverName
                    };
                }).ToList();

                Emails = new List<Message>(AllEmails);
                EmailDataGrid.ItemsSource = Emails;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading emails: {ex.Message}", "Error", MessageBoxButton.OK);
            }
        }

        #endregion

        #region ACTIONS

        private void EmailsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedEmail = EmailDataGrid.SelectedItem as Message;
        }

        private void EmailsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (EmailDataGrid.SelectedItem is Message email)
            {
                var detailsWindow = new DetailsCardWindow();
                detailsWindow.NavigateInsideDetailsCardWindow(new EmailDetailsCardView(email));
                detailsWindow.ShowDialog();
            }
        }

        private void EmailDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                EmailDataGrid.UnselectAll();
                Keyboard.ClearFocus();
                
                e.Handled = true;
            }
        }

        private void EmailFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = EmailFilterTextBox.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
                Emails = new List<Message>(AllEmails);
            else
            {
                Emails = AllEmails
                    .Where(email =>
                        (!string.IsNullOrEmpty(email.Subject) && email.Subject.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(email.Body) && email.Body.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(email.Email) && email.Email.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(email.ReceiverName) && email.ReceiverName.ToLower().Contains(searchText)) ||
                        email.SentAt.ToString("dd.MM.yyyy").Contains(searchText) ||
                        email.SentAt.ToString("dd/MM/yyyy").Contains(searchText)
                    )
                    .ToList();
            }

            for (int i = 0; i < Emails.Count; i++)
                Emails[i].RowNumber = i + 1;

            EmailDataGrid.ItemsSource = Emails;
        }

        #endregion
    }
}