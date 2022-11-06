using System;

// MassTransit uses namespaces as well - we need to keep the Publisher/Receiver bodies' namespaces the same
namespace MessageGenerator.MessageBodies
{
    public class Humidity
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public decimal Value { get; set; }

        public Humidity(int id, decimal value)
        {
            Id = id;
            Time = DateTime.UtcNow;
            Value = value;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Time)}: {Time}, {nameof(Value)}: {Value}";
        }
    }
}