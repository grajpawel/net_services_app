using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dashboard.Dtos;
namespace Dashboard.Data;

public class SensorsService
{
    private HttpClient _client;
    private readonly AppOptions _appOptions;

    public SensorsService(HttpClient client, AppOptions appOptions)
    {
        _client = client;
        _appOptions = appOptions;
    }

    public async Task<List<HumidityDto>> GetHumidityData()
    {
        using var response = await _client.GetAsync(_appOptions.HumidityApiLink);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<HumidityDto>>();
    }

    public async Task<List<PressureDto>> GetPressureData()
    {
        using var response = await _client.GetAsync(_appOptions.PressureApiLink);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<PressureDto>>();
    }

    public async Task<List<TemperatureDto>> GetTemperatureData()
    {
        using var response = await _client.GetAsync(_appOptions.TemperatureApiLink);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<TemperatureDto>>();
    }

    public async Task<List<WindDto>> GetWindData()
    {
        using var response = await _client.GetAsync(_appOptions.WindApiLink);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<WindDto>>();
    }
}