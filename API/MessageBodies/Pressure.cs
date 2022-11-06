using System;

namespace MessageGenerator.MessageBodies
{
    public class Pressure
    {
        private int Id { get; }
        private DateTime Time { get; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Time)}: {Time}, {nameof(Value)}: {Value}";
        }

        private decimal Value { get; }

        public Pressure(int id, decimal value)
        {
            Id = id;
            Time = DateTime.UtcNow;
            Value = value;
        }
    }
}