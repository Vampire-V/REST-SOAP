using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DemoSAP.Services
{
    public class SapService : ISapService
    {
        private readonly HttpClient _httpClient;

        // ใช้ HttpClient จาก Dependency Injection
        public SapService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetItems()
        {
            var response = await _httpClient.GetAsync("GetItems");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetItem(int id)
        {
            var response = await _httpClient.GetAsync($"GetItem/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreateItem(string requestBody)
        {
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("CreateItem", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateItem(int id, string requestBody)
        {
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"UpdateItem/{id}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"DeleteItem/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
