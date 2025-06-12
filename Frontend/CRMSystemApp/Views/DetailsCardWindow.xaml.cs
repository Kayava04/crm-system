using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace CRMSystemApp.Views
{
    public partial class DetailsCardWindow : Window
    {
        private bool _isClosing = false;

        public DetailsCardWindow()
        {
            InitializeComponent();

            Focus();
            KeyDown += DetailsCardWindow_KeyDown;
        }

        #region BASE METHODS

        private void CloseWindow_Click(object sender, RoutedEventArgs e) => Close();

        private void DetailsCardWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                FadeAndClose();
        }

        #endregion

        #region CHANGE VIEW

        public void NavigateInsideDetailsCardWindow(UserControl control)
        {
            DetailsCardContent.Content = control;

            if (control is StudentDetailsCardView)
            {
                Height = control.Height + 40;
                Width = control.Width;
            }
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