using System;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SampleMessageGenerator.message;

namespace SampleMessageGenerator.sensor
{
    public class Sensor4 : ISensor
    {
        private static readonly Random Global = new Random();
        [ThreadStatic] private static Random _local;
        
        public string Name { get; }
        public IModel Channel { get; }
        public int RangeFrom { get; set; }
        public int RangeTo { get; set; }
        public int N { get; set; }
        public int SleepTime { get; set; }

        public Sensor4(string name, IModel channel, int rangeFrom, int rangeTo, int n, int sleepTime)
        {
            Name = name;
            Channel = channel;
            RangeFrom = rangeFrom;
            RangeTo = rangeTo;
            N = n;
            SleepTime = sleepTime;
        }

        public async void GenerateMany()
        {
            for (int i = 0; i < N; i++)
            {
                var tmp = Draw(RangeFrom, RangeTo);
                IMessage message = new Message(Name, tmp);
                var json = JsonConvert.SerializeObject(message);
                Channel.BasicPublish(exchange: "", routingKey: "sensor4", body: Encoding.UTF8.GetBytes(json));
                Thread.Sleep(SleepTime);
            }
        }

        public void GenerateOne(int value)
        {
            IMessage message = new Message(Name, value);
            var json = JsonConvert.SerializeObject(message);
            Channel.BasicPublish(exchange: "", routingKey: "sensor4", body: Encoding.UTF8.GetBytes(json));
        }
        
        private int Draw(int rangeFrom, int rangeTo)
        {
            Init();
            return _local.Next(rangeFrom, rangeTo);
        }

        private void Init()
        {
            if (_local == null)
            {
                int seed;
                lock (Global)
                {
                    seed = Global.Next();
                }

                _local = new Random(seed);
            }
        }
    }
}
