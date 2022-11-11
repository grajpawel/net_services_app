using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Dtos
{
    public class WindDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public int SensorId { get; set; }
        public DateTime Time { get; set; }
        public decimal Speed { get; set; }
        public string Direction { get; set; }

        public WindDto(int sensorId, decimal speed, string direction)
        {
            SensorId = sensorId;
            Time = DateTime.UtcNow;
            Speed = speed;
            Direction = direction;
        }

        public override string ToString()
        {
            return $"{nameof(SensorId)}: {SensorId}, {nameof(Time)}: {Time}, {nameof(Speed)}: {Speed}, {nameof(Direction)}: {Direction}";
        }
    }
}