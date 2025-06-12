using CRMSystemApp.DTOs.User;
using CRMSystemApp.Models;

namespace CRMSystemApp.Services
{
    public static class UserService
    {
        public static async Task<User> GetCurrentUserAsync()
        {
            return await ApiService.GetAsync<User>("api/user/me");
        }

        public static async Task<bool> UpdateCurrentUserAsync(User user)
        {
            var updateUser = new
            {
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Email = user.Email,
                Username = user.Username
            };

            var response = await ApiService.PatchAsync("api/user/me", updateUser);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> UpdatePasswordAsync(UpdatePasswordDto passwordDto)
        {
            var response = await ApiService.PatchAsync("api/user/me/change-password", passwordDto);
            return response.IsSuccessStatusCode;
        }
    }
}