using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dashboard.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dashboard.Controllers;

public class DownloadController
{
    [Inject]
    private IJSRuntime JsRuntime { get; set; }

    public DownloadController(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }

    public async Task DownloadCsv(IEnumerable<ISensorDto> data)
    {
        var stream = GetCsvStream(data);
        const string fileName = "data.csv";

        using var streamRef = new DotNetStreamReference(stream: stream);
        await JsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }

    public async Task DownloadJson(IEnumerable<ISensorDto> data)
    {
        var stream = GetJsonStream(data);
        const string fileName = "data.json";

        using var streamRef = new DotNetStreamReference(stream: stream);
        await JsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }
    private static string CurrentDataToCsvFormat(IEnumerable<ISensorDto> currentData)
    {
        var csv = new StringBuilder();
        const string header = "sensor type;sensor id;read date;value;direction";
        csv.AppendLine(header);
        
        foreach (var record in currentData)
        {
            var direction = record.Direction == -1 ? "N.D" : record.Direction.ToString(CultureInfo.CurrentCulture);
            var line = $"{record.Type};{record.SensorId};{record.Time};{record.Value};{direction}";
            csv.AppendLine(line);
        }
        return csv.ToString();
    }

    private static string CurrentDataToJsonFormat(IEnumerable<ISensorDto> currentData)
    {
        var dto = new CurrentDataDto(currentData);
        var str = JsonSerializer.Serialize(dto);
        return str;
    }

    private static Stream GetCsvStream(IEnumerable<ISensorDto> data)
    {
        var tmp = Encoding.UTF8.GetBytes(CurrentDataToCsvFormat(data));
        return new MemoryStream(tmp);
    }

    private static Stream GetJsonStream(IEnumerable<ISensorDto> data)
    {
        var tmp = Encoding.UTF8.GetBytes(CurrentDataToJsonFormat(data));
        return new MemoryStream(tmp);
    }
}