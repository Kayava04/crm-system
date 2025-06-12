using System.Windows;
using System.Windows.Controls;

namespace CRMSystemApp.Views
{
    public partial class AboutView : UserControl
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = Window.GetWindow(this) as MainWindow;

            if (profileWindow != null)
                profileWindow.MainContent.Content = new HomeView();
        }
    }
}