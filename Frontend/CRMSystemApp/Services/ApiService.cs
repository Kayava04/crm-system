using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Text;

namespace CRMSystemApp.Services
{
    public static class ApiService
    {
        private static readonly HttpClientHandler handler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new CookieContainer(),
            AllowAutoRedirect = true
        };

        private static readonly HttpClient client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://localhost:7200/")
        };

        private static async Task<HttpResponseMessage> SendRequestAsync(Func<Task<HttpResponseMessage>> request)
        {
            var response = await request();

            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                AuthGuardService.SetAuthenticated(false);
                throw new UnauthorizedAccessException("Session expired. Redirecting to login...");
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode}, Message: {error}");
            }

            return response;
        }

        public static async Task<T> GetAsync<T>(string url)
        {
            var response = await SendRequestAsync(() => client.GetAsync(url));
            var json = await response.Content.ReadAsStringAsync();
            
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            return await SendRequestAsync(() => client.PostAsync(url, content));
        }

        public static async Task<HttpResponseMessage> PostFormAsync(string url, MultipartFormDataContent content)
        {
            return await SendRequestAsync(() => client.PostAsync(url, content));
        }

        public static async Task<HttpResponseMessage> PutAsync<T>(string url, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            return await SendRequestAsync(() => client.PutAsync(url, content));
        }

        public static async Task<HttpResponseMessage> PatchAsync<T>(string url, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = content
            };
            
            return await SendRequestAsync(() => client.SendAsync(request));
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await SendRequestAsync(() => client.DeleteAsync(url));
        }
    }
}