using System.Net.Http;
using System.Text.Json;
using CRMSystemApp.Models;

namespace CRMSystemApp.Services
{
    public static class UploadService
    {
        public static async Task<string> UploadPhotoAsync(MultipartFormDataContent formData)
        {
            var response = await ApiService.PostFormAsync("api/users/upload-photo", formData);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.ImageUrl;
        }

        //private class UploadPhotoResponse
        //{
        //    public string? ImageUrl { get; set; }
        //}
    }
}