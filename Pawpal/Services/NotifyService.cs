using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;

public class NotifyService
{
    private readonly HttpClient _httpClient;
    private const string apiUrl = "https://abys697-001-site1.otempurl.com/api/alert/status";

    public NotifyService()
    {
        _httpClient = new HttpClient();
    }
    
    public async Task<string> GetAlertStatusAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(apiUrl);
            return response;
        }
         catch (Exception ex)
        {
            var toast = Toast.Make($"API Error: {ex.Message}", CommunityToolkit.Maui.Core.ToastDuration.Short, 16);
            await toast.Show();
            return null;
        }
    }
}
