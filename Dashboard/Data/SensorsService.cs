using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Dashboard.Dtos;
using MessageGenerator.MessageBodies;

namespace Dashboard.Data;

public class SensorsService
{
    private HttpClient _client;

    public SensorsService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<HumidityDto>> GetHumidityData()
    {
        using HttpResponseMessage response = await _client.GetAsync("http://localhost:5001/api/Humidity");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<HumidityDto>>(json);
    }

    public async Task<List<PressureDto>> GetPressureData()
    {
        using HttpResponseMessage response = await _client.GetAsync("http://localhost:5001/api/Pressure");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<PressureDto>>(json);
    }

    public async Task<List<TemperatureDto>> GetTemperatureData()
    {
        using HttpResponseMessage response = await _client.GetAsync("http://localhost:5001/api/Temperature");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<TemperatureDto>>(json);
    }

    public async Task<List<WindDto>> GetWindData()
    {
        using HttpResponseMessage response = await _client.GetAsync("http://localhost:5001/api/Wind");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<WindDto>>(json);
    }
}