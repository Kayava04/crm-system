using System.Windows;
using CRMSystemApp.Services;
using CRMSystemApp.Views;

namespace CRMSystemApp
{
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            bool isLoggedIn = await AuthGuardService.ValidateAuthenticationAsync();

            if (isLoggedIn)
            {
                var main = new MainWindow();
                Current.MainWindow = main;

                main.Show();
            }
            else
            {
                var login = new LoginWindow();
                Current.MainWindow = login;

                login.Show();
            }
        }
    }
}