using System;
using System.Text.Json.Serialization;

namespace Dashboard.Dtos
{
    public class TemperatureDto : ISensorDto
    {
        public string _id { get; set; }
        
        public string type { get; set; }

        [JsonPropertyName("sensorId")]
        public int SensorId { get; set; }
        
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
        
        [JsonPropertyName("value")]
        public decimal Value { get; set; }
        
        public decimal Direction { get; set; }

        public TemperatureDto(int sensorId, decimal value)
        {
            SensorId = sensorId;
            Time = DateTime.UtcNow;
            Value = value;
            Direction = -1;
            type = "Temperature Sensor";
        }

        public override string ToString()
        {
            return $"{nameof(SensorId)}: {SensorId}, {nameof(Time)}: {Time}, {nameof(Value)}: {Value}";
        }
    }
}