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
    public partial class StudentsView : UserControl
    {
        private List<Student> AllStudents = [];
        private List<Student> Students = [];
        private Student? selectedStudent;

        public StudentsView()
        {
            InitializeComponent();

            _ = LoadStudents();
            StudentsDataGrid.PreviewKeyDown += StudentsDataGrid_PreviewKeyDown;
        }

        #region DATAGRID RENDERING

        private async Task LoadStudents()
        {
            try
            {
                var result = await StudentService.GetAllStudentsAsync();

                AllStudents = result
                    .OrderByDescending(c => c.SignDate)
                    .Select((emp, index) =>
                    {
                        emp.RowNumber = index + 1;

                        return emp;
                    }).ToList();

                Students = new List<Student>(AllStudents);
                StudentsDataGrid.ItemsSource = Students;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error", MessageBoxButton.OK);
            }
        }

        #endregion

        #region ACTIONS
        
        private void StudentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedStudent = StudentsDataGrid.SelectedItem as Student;
        }

        private void StudentsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem is Student student)
            {
                var detailsWindow = new DetailsCardWindow();
                detailsWindow.NavigateInsideDetailsCardWindow(new StudentDetailsCardView(student));
                detailsWindow.ShowDialog();
            }
        }

        private void StudentsDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                StudentsDataGrid.UnselectAll();
                Keyboard.ClearFocus();

                e.Handled = true;
            }
        }

        private void StudentFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = StudentFilterTextBox.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
                Students = new List<Student>(AllStudents);
            else
            {
                Students = AllStudents
                    .Where(st =>
                        (!string.IsNullOrEmpty(st.FullName) && st.FullName.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(st.Email) && st.Email.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(st.Phone) && st.Phone.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(st.ContractNumber) && st.ContractNumber.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(st.PaymentStatus) && st.PaymentStatus.ToLower().Contains(searchText)) ||
                        st.BirthDate.ToString("dd.MM.yyyy").Contains(searchText) ||
                        st.BirthDate.ToString("dd/MM/yyyy").Contains(searchText) ||
                        st.SignDate.ToString("dd.MM.yyyy").Contains(searchText) ||
                        st.SignDate.ToString("dd/MM/yyyy").Contains(searchText)
                    )
                    .ToList();
            }

            for (int i = 0; i < Students.Count; i++)
                Students[i].RowNumber = i + 1;

            StudentsDataGrid.ItemsSource = Students;
        }

        private async void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            var createEditWindow = new CreateEditWindow();
            var createEditView = new StudentCreateEditView();

            createEditWindow.NavigateInsideCreateEditWindow(createEditView);

            var result = createEditWindow.ShowDialog();
            if (result == true)
            {
                var student = createEditView.Student;
                var success = await StudentService.CreateStudentAsync(student);

                if (success)
                {
                    MessageBox.Show("Student created successfully!", "Success", MessageBoxButton.OK);
                    await LoadStudents();
                }
                else MessageBox.Show("Failed to create student.", "Error", MessageBoxButton.OK);
            }

            Keyboard.Focus(StudentsDataGrid);
        }

        private async void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            if (selectedStudent == null)
            {
                MessageBox.Show("Please select a student to edit.", "Warning", MessageBoxButton.OK);
                return;
            }

            var createEditWindow = new CreateEditWindow();
            var createEditView = new StudentCreateEditView(selectedStudent);

            createEditWindow.NavigateInsideCreateEditWindow(createEditView);

            var result = createEditWindow.ShowDialog();
            if (result == true)
            {
                var updatedStudent = createEditView.Student;
                var success = await StudentService.UpdateStudentAsync(selectedStudent.Id, updatedStudent);

                if (success)
                {
                    MessageBox.Show("Student updated successfully!", "Success", MessageBoxButton.OK);
                    await LoadStudents();
                }
                else MessageBox.Show("Failed to update student.", "Error", MessageBoxButton.OK);
            }

            Keyboard.Focus(StudentsDataGrid);
        }

        private async void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudents = StudentsDataGrid.SelectedItems.Cast<Student>().ToList();

            if (selectedStudents == null || selectedStudents.Count == 0)
            {
                MessageBox.Show("Please select one or more students to delete.", "Warning", MessageBoxButton.OK);
                return;
            }

            var message = selectedStudents.Count == 1
                ? $"Are you sure you want to delete {selectedStudents[0].FullName}?"
                : $"Are you sure you want to delete {selectedStudents.Count} students?";

            if (MessageBox.Show(message, "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int successCount = 0;

                foreach (var student in selectedStudents)
                {
                    var success = await StudentService.DeleteStudentAsync(student.Id);

                    if (success)
                        successCount++;
                }

                MessageBox.Show($"Deleted {successCount} of {selectedStudents.Count} students.", "Result", MessageBoxButton.OK);
                await LoadStudents();
            }

            Keyboard.Focus(StudentsDataGrid);
        }

        private async void ImportStudents_Click(object sender, RoutedEventArgs e)
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

                    var importedStudents = await StudentService.ImportStudentsAsync(content);

                    if (importedStudents != null && importedStudents.Any())
                        MessageBox.Show($"Imported {importedStudents.Count()} students successfully!", "Success", MessageBoxButton.OK);
                    else
                        MessageBox.Show("No students were imported.", "Information", MessageBoxButton.OK);

                    await LoadStudents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing students: {ex.Message}", "Error", MessageBoxButton.OK);
                }
            }
        }

        private async void ExportStudents_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save Students File",
                Filter = "Excel files (*.xlsx)|*.xlsx|JSON files (*.json)|*.json|CSV files (*.csv)|*.csv|All Files (*.*)|*.*",
                FileName = "StudentsExport.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var fileName = Path.GetFileName(saveFileDialog.FileName);
                    var fileData = await StudentService.ExportStudentsAsync(fileName);

                    await File.WriteAllBytesAsync(saveFileDialog.FileName, fileData);

                    MessageBox.Show("Students exported successfully!", "Success", MessageBoxButton.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting students: {ex.Message}", "Error", MessageBoxButton.OK);
                }
            }
        }

        #endregion
    }
}