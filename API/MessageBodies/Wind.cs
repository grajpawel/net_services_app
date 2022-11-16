using System;

// MassTransit uses namespaces as well - we need to keep the Publisher/Receiver bodies' namespaces the same
namespace MessageGenerator.MessageBodies
{
    public class Wind
    {
        public int SensorId { get; set; }
        public DateTime Time { get; set; }
        public decimal Speed { get; set; }
        public decimal Direction { get; set; }

        public Wind(int sensorId, decimal speed, decimal direction)
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