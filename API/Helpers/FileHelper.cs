using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using API.Dtos;

namespace API.Helpers;

public static class FileHelper
{
    public static string HumidityToCsv(IEnumerable<HumidityDto> data)
    {
        var csv = new StringBuilder();
        csv.AppendLine("_id;sensorId;time;value");

        foreach (var record in data)
        {
            csv.AppendLine($"{record._id};{record.SensorId};{record.Time};{record.Value}");
        }

        return csv.ToString();
    }

    public static string TemperatureToCsv(IEnumerable<TemperatureDto> data)
    {
        var csv = new StringBuilder();
        csv.AppendLine("_id;sensorId;time;value");

        foreach (var record in data)
        {
            csv.AppendLine($"{record._id};{record.SensorId};{record.Time};{record.Value}");
        }

        return csv.ToString();
    }

    public static string PressureToCsv(IEnumerable<PressureDto> data)
    {
        var csv = new StringBuilder();
        csv.AppendLine("_id;sensorId;time;value");

        foreach (var record in data)
        {
            csv.AppendLine($"{record._id};{record.SensorId};{record.Time};{record.Value}");
        }

        return csv.ToString();
    }

    public static string WindToCsv(IEnumerable<WindDto> data)
    {
        var csv = new StringBuilder();
        csv.AppendLine("_id;sensorId;time;speed;direction");

        foreach (var record in data)
        {
            csv.AppendLine($"{record._id};{record.SensorId};{record.Time};{record.Speed};{record.Direction}");
        }

        return csv.ToString();
    }

    public static string ListToJson<T>(IEnumerable<T> data)
    {
        var obj = JsonSerializer.Serialize(data);
        return "{\"data\":" + obj + "}";
    }
}