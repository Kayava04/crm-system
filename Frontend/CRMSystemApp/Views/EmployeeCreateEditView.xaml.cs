using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using CRMSystemApp.Extensions;
using CRMSystemApp.Models;

namespace CRMSystemApp.Views
{
    public partial class EmployeeCreateEditView : UserControl
    {
        public Employee Employee { get; private set; }

        public EmployeeCreateEditView()
        {
            InitializeComponent();

            Employee = new Employee();
            EmployeeCreateEditBirthDateSelector.SelectedDate = DateTime.UtcNow;
        }

        public EmployeeCreateEditView(Employee existingEmployee) : this()
        {
            Employee = existingEmployee;

            EmployeeCreateEditFullNameTextBox.Text = Employee.FullName;
            EmployeeCreateEditBirthDateSelector.SelectedDate = Employee.BirthDate;
            EmployeeCreateEditEmailTextBox.Text = Employee.Email;
            EmployeeCreateEditPhoneTextBox.Text = Employee.Phone;
            EmployeeCreateEditPositionTextBox.Text = Employee.Position;
            EmployeeCreateEditSalaryTextBox.Text = Employee.Salary.ToString(CultureInfo.InvariantCulture);
        }

        #region ACTIONS

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmployeeCreateEditFullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmployeeCreateEditEmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmployeeCreateEditPhoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmployeeCreateEditPositionTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmployeeCreateEditSalaryTextBox.Text) ||
                !EmployeeCreateEditBirthDateSelector.SelectedDate.HasValue)
            {
                MessageBox.Show("Please fill all the fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (EmployeeCreateEditBirthDateSelector.SelectedDate >= DateTime.Today)
            {
                MessageBox.Show("'Birth Date' must be in the past.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(EmployeeCreateEditSalaryTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var salary))
            {
                MessageBox.Show("Invalid salary format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Employee.FullName = EmployeeCreateEditFullNameTextBox.Text.Trim();
            Employee.BirthDate = EmployeeCreateEditBirthDateSelector.SelectedDate.ToUtcDate();
            Employee.Email = EmployeeCreateEditEmailTextBox.Text.Trim();
            Employee.Phone = EmployeeCreateEditPhoneTextBox.Text.Trim();
            Employee.Position = EmployeeCreateEditPositionTextBox.Text.Trim();
            Employee.Salary = salary;
            Employee.CreatedAt = DateTime.UtcNow;

            Window.GetWindow(this)!.DialogResult = true;
            Window.GetWindow(this)!.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)!.DialogResult = false;
            Window.GetWindow(this)!.Close();
        }

        #endregion
    }
}