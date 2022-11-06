using System;

// MassTransit uses namespaces as well - we need to keep the Publisher/Receiver bodies' namespaces the same
namespace MessageGenerator.MessageBodies
{
    public class Pressure
    {
        public int SensorId { get; }
        public DateTime Time { get; }

        public override string ToString()
        {
            return $"{nameof(SensorId)}: {SensorId}, {nameof(Time)}: {Time}, {nameof(Value)}: {Value}";
        }

        public decimal Value { get; }

        public Pressure(int sensorId, decimal value)
        {
            SensorId = sensorId;
            Time = DateTime.UtcNow;
            Value = value;
        }
    }
}