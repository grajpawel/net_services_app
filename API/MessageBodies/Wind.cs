using System;

namespace MessageGenerator.MessageBodies
{
    public class Wind
    {
        private int Id { get; }
        private DateTime Time { get; }
        private decimal Speed { get; }
        private string Direction { get; }

        public Wind(int id, decimal speed, string direction)
        {
            Id = id;
            Time = DateTime.UtcNow;
            Speed = speed;
            Direction = direction;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Time)}: {Time}, {nameof(Speed)}: {Speed}, {nameof(Direction)}: {Direction}";
        }
    }
}