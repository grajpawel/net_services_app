using System;

namespace MessageGenerator.MessageBodies
{
    public class Humidity
    {
        public int SensorId { get; set; }
        public DateTime Time { get; set; }
        public decimal Value { get; set; }

        public Humidity(int sensorId, decimal value)
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