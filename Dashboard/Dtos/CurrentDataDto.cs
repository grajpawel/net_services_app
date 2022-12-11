using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Dashboard.Dtos;

public class CurrentDataDto
{
    [JsonPropertyName("data")]
    public IEnumerable<ISensorDto> Data { get; set; }
    public CurrentDataDto(IEnumerable<ISensorDto> data)
    {
        Data = data;
    }
}