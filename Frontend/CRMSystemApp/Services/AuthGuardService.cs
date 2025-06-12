namespace CRMSystemApp.Services
{
    public static class AuthGuardService
    {
        private static bool _isAuthenticated = false;

        public static bool IsAuthenticated => _isAuthenticated;

        public static void SetAuthenticated(bool value)
        {
            _isAuthenticated = value;
        }

        public static async Task<bool> ValidateAuthenticationAsync()
        {
            try
            {
                var user = await UserService.GetCurrentUserAsync();
                _isAuthenticated = user != null;
            }
            catch
            {
                _isAuthenticated = false;
            }

            return _isAuthenticated;
        }
    }
}