using Pawpal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Pawpal.Services
{
    class FeedingTimeService : IFeedingTimeService
    {
        private readonly HttpClient _httpClient;
        private readonly string apiUrl = "https://abys697-001-site1.otempurl.com/api/time";

        public FeedingTimeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<FeedingTimeEntry>> GetAllFeedingTimeAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<FeedingTimeEntry>>(apiUrl);
        }

        public async Task AddFeedingTimeAsync(FeedingTimeEntry entry)
        {
            var response = await _httpClient.PostAsJsonAsync(apiUrl, entry);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteFeedingTimeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{apiUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
