using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace CRMSystemApp.Views
{
    public partial class CreateEditWindow : Window
    {
        private const string STUDENT_CREATE_EDIT_WINDOW_TITLE = "Student";
        private const string EMPLOYEE_CREATE_EDIT_WINDOW_TITLE = "Employee";
        private bool _isClosing = false;

        public CreateEditWindow()
        {
            InitializeComponent();
            
            Focus();
            KeyDown += CreateEditWindow_KeyDown;
        }

        #region BASE METHODS

        private void MoveWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void CloseWindow_Click(object sender, RoutedEventArgs e) => Close();

        private void CreateEditWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                FadeAndClose();
        }

        #endregion

        #region CHANGE VIEW

        public void NavigateInsideCreateEditWindow(UserControl control)
        {
            CreateEditContent.Content = control;

            if (control is StudentCreateEditView)
            {
                CreateEditWindowTitle.Text = STUDENT_CREATE_EDIT_WINDOW_TITLE;

                Height = control.Height + 40;
                Width = control.Width;
            }

            else if (control is EmployeeCreateEditView)
                CreateEditWindowTitle.Text = EMPLOYEE_CREATE_EDIT_WINDOW_TITLE;
        }

        #endregion

        #region RENDERING WINDOW

        private void FadeAndClose()
        {
            if (_isClosing)
                return;

            _isClosing = true;

            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                FillBehavior = FillBehavior.Stop
            };

            fadeOutAnimation.Completed += (s, e) => Close();
            BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        #endregion
    }
}