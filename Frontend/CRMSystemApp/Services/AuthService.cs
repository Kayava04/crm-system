namespace CRMSystemApp.Services
{
    public static class AuthService
    {
        public static async Task<bool> LoginAsync(string username, string password)
        {
            var user = new
            {
                Username = username,
                Password = password
            };

            var response = await ApiService.PostAsync("api/auth/sign-in", user);
            
            if (response.IsSuccessStatusCode)
            {
                AuthGuardService.SetAuthenticated(true);
                return true;
            }

            return false;
        }

        public static async Task<bool> RegisterAsync(string fullName, DateTime birthDate, string email, string username, string password, string confirmPassword)
        {
            var utcBirthDate = DateTime.SpecifyKind(birthDate.Date, DateTimeKind.Utc);

            var user = new
            {
                FullName = fullName,
                BirthDate = utcBirthDate,
                Email = email,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var response = await ApiService.PostAsync("api/auth/sign-up", user);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> LogoutAsync()
        {
            var response = await ApiService.PostAsync("api/auth/sign-out", new { });
            return response.IsSuccessStatusCode;
        }
    }
}