using System;

// MassTransit uses namespaces as well - we need to keep the Publisher/Receiver bodies' namespaces the same
namespace MessageGenerator.MessageBodies
{
    public class Temperature
    {
        public int SensorId { get; }
        public DateTime Time { get; }
        public decimal Value { get; }

        public Temperature(int sensorId, decimal value)
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