using System.Net.Http;
using System.Text.Json;
using CRMSystemApp.Models;

namespace CRMSystemApp.Services
{
    public static class StudentService
    {
        public static async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await ApiService.GetAsync<IEnumerable<Student>>("api/student-registration");
        }

        public static async Task<Student> GetStudentByIdAsync(Guid id)
        {
            return await ApiService.GetAsync<Student>($"api/student-registration/{id}");
        }

        public static async Task<bool> CreateStudentAsync(Student student)
        {
            FixStudentBeforeSend(student);

            var response = await ApiService.PostAsync("api/student-registration", student);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> UpdateStudentAsync(Guid id, Student student)
        {
            FixStudentBeforeSend(student);

            var response = await ApiService.PutAsync($"api/student-registration/{id}", student);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> DeleteStudentAsync(Guid id)
        {
            var response = await ApiService.DeleteAsync($"api/student-registration/{id}");
            return response.IsSuccessStatusCode;
        }

        public static async Task<IEnumerable<Student>> ImportStudentsAsync(MultipartFormDataContent formData)
        {
            var response = await ApiService.PostFormAsync("api/student-registration/import-file", formData);
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<Student>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? [];
        }

        public static async Task<byte[]> ExportStudentsAsync(string fileName)
        {
            var students = await GetAllStudentsAsync();

            foreach (var student in students)
                FixStudentBeforeSend(student);

            var json = JsonSerializer.Serialize(students, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            var content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await ApiService.PostAsync($"api/student-registration/export-file?fileName={fileName}", content);
            return await response.Content.ReadAsByteArrayAsync();
        }

        private static void FixStudentBeforeSend(Student student)
        {
            if (student.Grade is < 0)
                student.Grade = null;

            student.IsParentRegister = !string.IsNullOrWhiteSpace(student.ParentFullName);
            student.ParentFullName = student.IsParentRegister ? student.ParentFullName?.Trim() : null;
        }
    }
}