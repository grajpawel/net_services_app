using System;
using System.Text.Json.Serialization;

namespace Dashboard.Dtos
{
    public class PressureDto : ISensorDto
    {
        public string _id { get; set; }
        
        public string Type { get; set; }

        [JsonPropertyName("sensorId")]
        public int SensorId { get; set; }
        
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("value")]
        public decimal Value { get; set; }

        public decimal Direction { get; set; }

        public PressureDto(int sensorId, decimal value, DateTime time)
        {
            SensorId = sensorId;
            Time = time;
            Value = value;
            Direction = -1;
            Type = "Pressure Sensor";
        }

        public override string ToString()
        {
            return $"{nameof(SensorId)}: {SensorId}, {nameof(Time)}: {Time}, {nameof(Value)}: {Value}";
        }
    }
}