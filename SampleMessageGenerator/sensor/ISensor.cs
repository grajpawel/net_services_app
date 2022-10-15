using RabbitMQ.Client;

namespace SampleMessageGenerator.sensor
{
    public interface ISensor
    {
        string Name { get; }
        IModel Channel { get; }
        int RangeFrom { get; set; }
        int RangeTo { get; set; }
        int N { get; set; }
        int SleepTime { get; set; }
        void GenerateMany();
        void GenerateOne(int value);
    }
}
