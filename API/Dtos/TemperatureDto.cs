using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Dtos
{
    public class TemperatureDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public int SensorId { get; set; }
        public DateTime Time { get; set; }
        public decimal Value { get; set; }

        public TemperatureDto(int sensorId, decimal value)
        {
            SensorId = sensorId;
            Time = DateTime.UtcNow;
            Value = value;
        }

        public override string ToString()
        {
            return $"{nameof(SensorId)}: {SensorId}, {nameof(Time)}: {Time}, {nameof(Value)}: {Value}";
        }
    }
}