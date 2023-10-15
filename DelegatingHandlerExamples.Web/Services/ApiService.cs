using DelegatingHandlerExamples.Shared.Models;

namespace DelegatingHandlerExamples.Web.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<WeatherForecast>?> GetWeatherForecast()
    {
        var response = await _httpClient.GetAsync("WeatherForecast");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        }
        else
        {
            string responseBody = await response?.Content?.ReadAsStringAsync();
            throw new Exception($"{response?.StatusCode}: {responseBody}");
        }
    }
}
