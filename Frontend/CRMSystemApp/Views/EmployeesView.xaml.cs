using Microsoft.Win32;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRMSystemApp.Models;
using CRMSystemApp.Services;

namespace CRMSystemApp.Views
{
    public partial class EmployeesView : UserControl
    {
        private List<Employee> AllEmployees = [];
        private List<Employee> Employees = [];
        private Employee? selectedEmployee;

        public EmployeesView()
        {
            InitializeComponent();

            _ = LoadEmployees();
            EmployeesDataGrid.PreviewKeyDown += EmployeesDataGrid_PreviewKeyDown;
        }

        #region DATAGRID RENDERING

        private async Task LoadEmployees()
        {
            try
            {
                var result = await EmployeeService.GetAllEmployeesAsync();
                
                AllEmployees = result
                    .OrderByDescending(c => c.CreatedAt)
                    .Select((emp, index) =>
                    {
                        emp.RowNumber = index + 1;
                        
                        return emp;
                    }).ToList();
                
                Employees = new List<Employee>(AllEmployees);
                EmployeesDataGrid.ItemsSource = Employees;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButton.OK);
            }
        }

        #endregion

        #region ACTIONS

        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEmployee = EmployeesDataGrid.SelectedItem as Employee;
        }

        private void EmployeesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem is Employee employee)
            {
                var detailsWindow = new DetailsCardWindow();
                detailsWindow.NavigateInsideDetailsCardWindow(new EmployeeDetailsCardView(employee));
                detailsWindow.ShowDialog();
            }
        }

        private void EmployeesDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                EmployeesDataGrid.UnselectAll();
                Keyboard.ClearFocus();
                
                e.Handled = true;
            }
        }

        private void EmployeeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = EmployeeFilterTextBox.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
                Employees = new List<Employee>(AllEmployees);
            else
            {
                Employees = AllEmployees
                    .Where(emp =>
                        (!string.IsNullOrEmpty(emp.FullName) && emp.FullName.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(emp.Email) && emp.Email.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(emp.Phone) && emp.Phone.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(emp.Position) && emp.Position.ToLower().Contains(searchText)) ||
                        emp.BirthDate.ToString("dd.MM.yyyy").Contains(searchText) ||
                        emp.BirthDate.ToString("dd/MM/yyyy").Contains(searchText) ||
                        emp.Salary.ToString().Contains(searchText)
                    )
                    .ToList();
            }

            for (int i = 0; i < Employees.Count; i++)
                Employees[i].RowNumber = i + 1;

            EmployeesDataGrid.ItemsSource = Employees;
        }

        private async void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var createEditWindow = new CreateEditWindow();
            var createEditView = new EmployeeCreateEditView();
            
            createEditWindow.NavigateInsideCreateEditWindow(createEditView);

            var result = createEditWindow.ShowDialog();
            if (result == true)
            {
                var employee = createEditView.Employee;
                var success = await EmployeeService.CreateEmployeeAsync(employee);

                if (success)
                {
                    MessageBox.Show("Employee created successfully!", "Success", MessageBoxButton.OK);
                    await LoadEmployees();
                }
                else MessageBox.Show("Failed to create employee.", "Error", MessageBoxButton.OK);
            }

            Keyboard.Focus(EmployeesDataGrid);
        }

        private async void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEmployee == null)
            {
                MessageBox.Show("Please select an employee to edit.", "Warning", MessageBoxButton.OK);
                return;
            }

            var createEditWindow = new CreateEditWindow();
            var createEditView = new EmployeeCreateEditView(selectedEmployee);
            
            createEditWindow.NavigateInsideCreateEditWindow(createEditView);

            var result = createEditWindow.ShowDialog();
            if (result == true)
            {
                var updatedEmployee = createEditView.Employee;
                var success = await EmployeeService.UpdateEmployeeAsync(selectedEmployee.Id, updatedEmployee);

                if (success)
                {
                    MessageBox.Show("Employee updated successfully!", "Success", MessageBoxButton.OK);
                    await LoadEmployees();
                }
                else MessageBox.Show("Failed to update employee.", "Error", MessageBoxButton.OK);
            }

            Keyboard.Focus(EmployeesDataGrid);
        }

        private async void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployees = EmployeesDataGrid.SelectedItems.Cast<Employee>().ToList();

            if (selectedEmployees == null || selectedEmployees.Count == 0)
            {
                MessageBox.Show("Please select one or more employees to delete.", "Warning", MessageBoxButton.OK);
                return;
            }

            var message = selectedEmployees.Count == 1
                ? $"Are you sure you want to delete {selectedEmployees[0].FullName}?"
                : $"Are you sure you want to delete {selectedEmployees.Count} employees?";

            if (MessageBox.Show(message, "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int successCount = 0;

                foreach (var employee in selectedEmployees)
                {
                    var success = await EmployeeService.DeleteEmployeeAsync(employee.Id);

                    if (success)
                        successCount++;
                }

                MessageBox.Show($"Deleted {successCount} of {selectedEmployees.Count} employees.", "Result", MessageBoxButton.OK);
                await LoadEmployees();
            }

            Keyboard.Focus(EmployeesDataGrid);
        }

        private async void ImportEmployees_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select a file to import",
                Filter = "Excel files (*.xlsx)|*.xlsx|JSON files (*.json)|*.json|CSV files (*.csv)|*.csv|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var filePath = openFileDialog.FileName;
                    var content = new MultipartFormDataContent();
                    var fileBytes = await File.ReadAllBytesAsync(filePath);
                    var byteContent = new ByteArrayContent(fileBytes);
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    content.Add(byteContent, "file", Path.GetFileName(filePath));

                    var importedEmployees = await EmployeeService.ImportEmployeesAsync(content);

                    if (importedEmployees != null && importedEmployees.Any())
                        MessageBox.Show($"Imported {importedEmployees.Count()} employees successfully!", "Success", MessageBoxButton.OK);
                    else
                        MessageBox.Show("No employees were imported.", "Information", MessageBoxButton.OK);

                    await LoadEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing employees: {ex.Message}", "Error", MessageBoxButton.OK);
                }
            }
        }

        private async void ExportEmployees_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save Employees File",
                Filter = "Excel files (*.xlsx)|*.xlsx|JSON files (*.json)|*.json|CSV files (*.csv)|*.csv|All Files (*.*)|*.*",
                FileName = "EmployeesExport.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var fileName = Path.GetFileName(saveFileDialog.FileName);
                    var fileData = await EmployeeService.ExportEmployeesAsync(fileName);

                    await File.WriteAllBytesAsync(saveFileDialog.FileName, fileData);

                    MessageBox.Show("Employees exported successfully!", "Success", MessageBoxButton.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting employees: {ex.Message}", "Error", MessageBoxButton.OK);
                }
            }
        }

        #endregion
    }
}