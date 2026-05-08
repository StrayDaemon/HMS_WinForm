using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HospitalManagementSystem.Core
{
    public static class ApiClient
    {
        // ✅ Change this to your XAMPP PHP backend URL
        private const string BaseUrl = "http://localhost/hms_api/";

        private static readonly HttpClient _client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        // ─── GET ───────────────────────────────────────────────
        public static async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                string url = BaseUrl + endpoint;
                HttpResponseMessage response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception($"GET failed [{endpoint}]: {ex.Message}");
            }
        }

        // ─── POST ──────────────────────────────────────────────
        public static async Task<T> PostAsync<T>(string endpoint, Dictionary<string, string> formData)
        {
            try
            {
                string url = BaseUrl + endpoint;
                var content = new FormUrlEncodedContent(formData);

                HttpResponseMessage response = await _client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception($"POST failed [{endpoint}]: {ex.Message}");
            }
        }

        // ─── Generic API Response wrapper ──────────────────────
        // Every PHP response should return: { "success": true/false, "message": "...", "data": ... }
    }

    // Standard response shape from PHP
    public class ApiResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    // For responses with no data payload
    public class ApiResponse : ApiResponse<object> { }
}
