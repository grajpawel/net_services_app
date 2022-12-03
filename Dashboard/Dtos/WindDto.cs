using System;
using System.Text.Json.Serialization;

namespace Dashboard.Dtos
{
    public class WindDto : ISensorDto
    {
        public string _id { get; set; }
        
        public string type { get; set; }

        [JsonPropertyName("sensorId")]
        public int SensorId { get; set; }
        
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("speed")]
        public decimal Value { get; set; }
        
        [JsonPropertyName("direction")]
        public decimal Direction { get; set; }

        [JsonConstructor]
        public WindDto(int sensorId, decimal value, decimal direction, DateTime time)
        {
            SensorId = sensorId;
            Time = time;
            Value = value;
            Direction = direction;
            type = "Wind Sensor";
        }

        public override string ToString()
        {
            return $"{nameof(SensorId)}: {SensorId}, {nameof(Time)}: {Time}, {nameof(Value)}: {Value}, {nameof(Direction)}: {Direction}";
        }
    }
}