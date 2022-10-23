using System;

namespace MessageGenerator.MessageBodies
{
    public class Temperature
    {
        private int Id { get; }
        private DateTime Time { get; }
        private decimal Value { get; }

        public Temperature(int id, decimal value)
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