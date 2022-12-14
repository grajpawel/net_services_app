using System;

namespace MessageGenerator.MessageBodies
{
    public class Pressure
    {
        public int SensorId { get; }
        public DateTime Time { get; }

        public decimal Value { get; }

        public Pressure(int sensorId, decimal value)
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