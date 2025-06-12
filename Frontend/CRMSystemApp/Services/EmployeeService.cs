using System.Net.Http;
using System.Text.Json;
using CRMSystemApp.Models;

namespace CRMSystemApp.Services
{
    public static class EmployeeService
    {
        public static async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await ApiService.GetAsync<IEnumerable<Employee>>("api/employee-registration");
        }

        public static async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            return await ApiService.GetAsync<Employee>($"api/employee-registration/{id}");
        }

        public static async Task<bool> CreateEmployeeAsync(Employee employee)
        {
            var response = await ApiService.PostAsync("api/employee-registration", employee);
            
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> UpdateEmployeeAsync(Guid id, Employee employee)
        {
            var response = await ApiService.PutAsync($"api/employee-registration/{id}", employee);
            
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> DeleteEmployeeAsync(Guid id)
        {
            var response = await ApiService.DeleteAsync($"api/employee-registration/{id}");
            
            return response.IsSuccessStatusCode;
        }

        public static async Task<IEnumerable<Employee>> ImportEmployeesAsync(MultipartFormDataContent formData)
        {
            var response = await ApiService.PostFormAsync("api/employee-registration/import-file", formData);
            var json = await response.Content.ReadAsStringAsync();
            
            return JsonSerializer.Deserialize<IEnumerable<Employee>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static async Task<byte[]> ExportEmployeesAsync(string fileName)
        {
            var response = await ApiService.PostAsync<object>($"api/employee-registration/export-file?fileName={fileName}", new { });
            
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}