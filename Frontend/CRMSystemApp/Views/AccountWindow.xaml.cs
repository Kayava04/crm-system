using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace CRMSystemApp.Views
{
    public partial class AccountWindow : Window
    {
        private const string ACCOUNT_WINDOW_TITLE = "Account";
        private const string PASSWORD_WINDOW_TITLE = "Password";
        private bool _isClosing = false;

        public AccountWindow()
        {
            InitializeComponent();

            NavigateInsideAccount(new AccountView());
            
            Focus();
            KeyDown += AccountWindow_KeyDown;
        }

        #region BASE METHODS

        private void MoveWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void CloseWindow_Click(object sender, RoutedEventArgs e) => Close();

        private void AccountWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                FadeAndClose();
        }

        #endregion

        #region CHANGE VIEW

        public void NavigateInsideAccount(UserControl control)
        {
            AccountContent.Content = control;

            if (control is ChangePasswordView)
                WindowTitle.Text = PASSWORD_WINDOW_TITLE;
            
            else if (control is AccountView)
                WindowTitle.Text = ACCOUNT_WINDOW_TITLE;
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