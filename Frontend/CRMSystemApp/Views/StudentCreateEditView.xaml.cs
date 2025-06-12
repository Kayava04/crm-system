using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using CRMSystemApp.Extensions;
using CRMSystemApp.Models;

namespace CRMSystemApp.Views
{
    public partial class StudentCreateEditView : UserControl
    {
        public Student Student { get; private set; }

        public StudentCreateEditView()
        {
            InitializeComponent();

            Student = new Student();
            StudentCreateEditBirthDateSelector.SelectedDate = DateTime.UtcNow;
            StudentCreateEditSignDateSelector.SelectedDate = DateTime.UtcNow;
        }

        public StudentCreateEditView(Student existingStudent) : this()
        {
            Student = existingStudent;

            StudentCreateEditParentFullNameTextBox.Text = Student.ParentFullName ?? string.Empty;
            StudentCreateEditFullNameTextBox.Text = Student.FullName;
            StudentCreateEditBirthDateSelector.SelectedDate = Student.BirthDate;
            StudentCreateEditGradeTextBox.Text = Student.Grade.HasValue ? Student.Grade.ToString() : string.Empty;
            StudentCreateEditEmailTextBox.Text = Student.Email;
            StudentCreateEditPhoneTextBox.Text = Student.Phone;
            StudentCreateEditLanguageTextBox.Text = Student.LanguageName;
            StudentCreateEditLevelTextBox.Text = Student.Level;
            StudentCreateEditGroupTextBox.Text = Student.GroupName;
            StudentCreateEditLessonDaysTextBox.Text = Student.LessonDays;
            StudentCreateEditPairTextBox.Text = Student.PairNumber.ToString();
            StudentCreateEditSignDateSelector.SelectedDate = Student.SignDate;
            StudentCreateEditPaymentAmmountTextBox.Text = Student.PaymentAmount.ToString(CultureInfo.InvariantCulture);

            StudentCreateEditPaymentStatusComboBox.SelectedIndex = Student.IsPaid ? 0 : 1;
        }

        #region ACTIONS

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StudentCreateEditFullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(StudentCreateEditEmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(StudentCreateEditPhoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(StudentCreateEditPaymentAmmountTextBox.Text) ||
                !StudentCreateEditBirthDateSelector.SelectedDate.HasValue ||
                !StudentCreateEditSignDateSelector.SelectedDate.HasValue ||
                StudentCreateEditPaymentStatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill all the fields.", "Validation Error", MessageBoxButton.OK);
                return;
            }

            if (StudentCreateEditBirthDateSelector.SelectedDate >= DateTime.Today)
            {
                MessageBox.Show("'Birth Date' must be in the past.", "Validation Error", MessageBoxButton.OK);
                return;
            }

            if (!decimal.TryParse(StudentCreateEditPaymentAmmountTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var paymentAmmount))
            {
                MessageBox.Show("Invalid payment format.", "Validation Error", MessageBoxButton.OK);
                return;
            }

            var paymentStatus = (StudentCreateEditPaymentStatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString()?.ToLower();

            if (paymentStatus is not ("paid" or "unpaid"))
            {
                MessageBox.Show("Payment Status must be either 'Paid' or 'Unpaid'.", "Validation Error", MessageBoxButton.OK);
                return;
            }

            string parentFullName = StudentCreateEditParentFullNameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(parentFullName))
            {
                Student.ParentFullName = null;
                Student.IsParentRegister = false;
            }
            else
            {
                Student.ParentFullName = parentFullName;
                Student.IsParentRegister = true;
            }

            string gradeText = StudentCreateEditGradeTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(gradeText))
                Student.Grade = null;
            else if (int.TryParse(gradeText, out int parsedGrade))
                Student.Grade = parsedGrade;
            else
            {
                MessageBox.Show("Grade must be a valid number or left empty.", "Validation Error", MessageBoxButton.OK);
                return;
            }

            Student.FullName = StudentCreateEditFullNameTextBox.Text.Trim();
            Student.BirthDate = StudentCreateEditBirthDateSelector.SelectedDate.Value.ToUtcDate();
            Student.Email = StudentCreateEditEmailTextBox.Text.Trim();
            Student.Phone = StudentCreateEditPhoneTextBox.Text.Trim();
            Student.LanguageName = StudentCreateEditLanguageTextBox.Text.Trim();
            Student.Level = StudentCreateEditLevelTextBox.Text.Trim();
            Student.GroupName = StudentCreateEditGroupTextBox.Text.Trim();
            Student.LessonDays = StudentCreateEditLessonDaysTextBox.Text.Trim();
            Student.PairNumber = int.TryParse(StudentCreateEditPairTextBox.Text.Trim(), out int pair) ? pair : 0;
            Student.SignDate = StudentCreateEditSignDateSelector.SelectedDate.Value.ToUtcDate();
            Student.PaymentAmount = paymentAmmount;
            Student.IsPaid = paymentStatus == "paid";
            Student.SignDate = DateTime.UtcNow;

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