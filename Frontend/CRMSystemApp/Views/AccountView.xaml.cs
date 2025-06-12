using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CRMSystemApp.Extensions;
using CRMSystemApp.Models;
using CRMSystemApp.Services;

namespace CRMSystemApp.Views
{
    public partial class AccountView : UserControl
    {
        public AccountView()
        {
            InitializeComponent();

            LoadUserProfileInfo();
        }

        #region ACTIONS

        private async void UploadProfilePhoto_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select a Photo",
                Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var filePath = openFileDialog.FileName;
                    var fileBytes = await File.ReadAllBytesAsync(filePath);
                    var fileName = Path.GetFileName(filePath);

                    var content = new MultipartFormDataContent();
                    var byteContent = new ByteArrayContent(fileBytes);
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                    content.Add(byteContent, "file", fileName);

                    var imageUrl = await UploadService.UploadPhotoAsync(content);

                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        ShowProfilePhotoOrInitials(imageUrl, FullNameTextBox.Text);

                        MessageBox.Show("Photo uploaded successfully!", "Success", MessageBoxButton.OK);
                    }
                    else
                        MessageBox.Show("Failed to upload photo.", "Error", MessageBoxButton.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading photo: {ex.Message}", "Error", MessageBoxButton.OK);
                }
            }
        }

        private void EditProfileInfo_Click(object sender, RoutedEventArgs e)
        {
            FullNameTextBox.IsReadOnly = false;
            EmailTextBox.IsReadOnly = false;
            Blocker.Visibility = Visibility.Collapsed;
            SaveChangesButton.Visibility = Visibility.Visible;
        }

        private async void SaveProfileChanges_Click(object sender, RoutedEventArgs e)
        {
            var updatedUser = new User
            {
                FullName = FullNameTextBox.Text,
                Email = EmailTextBox.Text,
                Username = UsernameTextBox.Text,
                BirthDate = BirthDateSelector.SelectedDate.ToUtcDate()
            };

            try
            {
                var success = await UserService.UpdateCurrentUserAsync(updatedUser);

                if (success)
                {
                    FullNameTextBox.IsReadOnly = true;
                    EmailTextBox.IsReadOnly = true;
                    Blocker.Visibility = Visibility.Visible;
                    SaveChangesButton.Visibility= Visibility.Collapsed;

                    var currentUser = await UserService.GetCurrentUserAsync();
                    ShowProfilePhotoOrInitials(currentUser.ImageUrl, updatedUser.FullName);

                    MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK);
                }
                else MessageBox.Show("Failed to update profile.", "Error", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while updating profile: {ex.Message}", "Error", MessageBoxButton.OK);
            }
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is AccountWindow window)
                window.NavigateInsideAccount(new ChangePasswordView());
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var success = await AuthService.LogoutAsync();

                if (success)
                {
                    AuthGuardService.SetAuthenticated(false);

                    var accountWindow = Window.GetWindow(this);
                    accountWindow?.Close();

                    var login = new LoginWindow();
                    Application.Current.MainWindow = login;
                    login.Show();

                    Application.Current.Windows
                        .OfType<MainWindow>()
                        .FirstOrDefault()
                        ?.Close();
                }
                else MessageBox.Show("Logout failed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during logout: {ex.Message}", "Error", MessageBoxButton.OK);
            }
        }

        #endregion

        #region USER PROFILE RENDERING

        private async void LoadUserProfileInfo()
        {
            try
            {
                var user = await UserService.GetCurrentUserAsync();

                if (user != null)
                {
                    UsernameTextBox.Text = user.Username ?? string.Empty;
                    FullNameTextBox.Text = user.FullName ?? string.Empty;
                    EmailTextBox.Text = user.Email ?? string.Empty;
                    BirthDateSelector.SelectedDate = user.BirthDate.Date;

                    ShowProfilePhotoOrInitials(user.ImageUrl, user.FullName);
                }
                else MessageBox.Show("Failed to load 'User' data.", "Error", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading 'User' data: {ex.Message}", "Error", MessageBoxButton.OK);
            }
        }

        private void ShowProfilePhotoOrInitials(string? photoUrl, string? fullName)
        {
            if (!string.IsNullOrWhiteSpace(photoUrl))
            {
                try
                {
                    var localPath = ConvertImageUrlToLocalPath(photoUrl);

                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(localPath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    ProfileImage.Source = bitmap;
                    ProfileImage.Visibility = Visibility.Visible;
                    InitialsTextBlock.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    ShowUserInitials(fullName);
                }
            }
            else ShowUserInitials(fullName);
        }


        private void ShowUserInitials(string? fullName)
        {
            fullName = fullName?.Trim();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                InitialsTextBlock.Text = "";
                ProfileImage.Visibility = Visibility.Collapsed;
                InitialsTextBlock.Visibility = Visibility.Visible;
                return;
            }

            var parts = fullName
                .Split([' '], StringSplitOptions.RemoveEmptyEntries)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToArray();

            string initials = "";

            if (parts.Length >= 1)
                initials += char.ToUpper(parts[0][0]);

            if (parts.Length >= 2)
                initials += char.ToUpper(parts[1][0]);

            InitialsTextBlock.Text = initials;

            InitialsTextBlock.Text = initials;
            InitialsTextBlock.Visibility = Visibility.Visible;
            ProfileImage.Visibility = Visibility.Collapsed;
        }

        private string ConvertImageUrlToLocalPath(string imageUrl)
        {
            var fileName = Path.GetFileName(imageUrl);
            var uploadsDirectory = @"D:\Projects\C#\CRMSystem\Backend\CRMSystem.WebAPI\Uploads";
            
            return Path.Combine(uploadsDirectory, fileName);
        }

        #endregion
    }
}