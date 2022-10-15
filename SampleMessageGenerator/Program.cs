using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using SampleMessageGenerator.sensor;

namespace SampleMessageGenerator
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var flag = Environment.GetEnvironmentVariable("mode");
            if (flag == null || (!flag.Equals("many") && !flag.Equals("one")))
            {
                Console.WriteLine($"wrong flag: {flag}");
                return;
            }

            var factory = new ConnectionFactory
            {
                HostName = "net.services.rabbitmq"
            };

            var connection = factory.CreateConnection();

            if (flag.Equals("many"))
            {
                await GenerateManyMode(connection);
            }
            else
            {
                GenerateOneMode(connection);
            }
        }

        private static void GenerateOneMode(IConnection connection)
        {
            var type = int.Parse(Environment.GetEnvironmentVariable("type") ??
                                 throw new InvalidOperationException("type"));
            var value = int.Parse(Environment.GetEnvironmentVariable("value") ??
                                  throw new InvalidOperationException("value"));

            if (type.Equals(1))
            {
                var channel = connection.CreateModel();
                channel.QueueDeclare("sensor1", durable: false, exclusive: false, autoDelete: false, arguments: null);
                var sensor = new Sensor1("tmp", channel, value, value, 1, 0);
                sensor.GenerateOne(value);
            } else if (type.Equals(2))
            {
                var channel = connection.CreateModel();
                channel.QueueDeclare("sensor2", durable: false, exclusive: false, autoDelete: false, arguments: null);
                var sensor = new Sensor2("tmp", channel, value, value, 1, 0);
                sensor.GenerateOne(value);
            } else if (type.Equals(3))
            {
                var channel = connection.CreateModel();
                channel.QueueDeclare("sensor3", durable: false, exclusive: false, autoDelete: false, arguments: null);
                var sensor = new Sensor3("tmp", channel, value, value, 1, 0);
                sensor.GenerateOne(value);
            } else if (type.Equals(4))
            {
                var channel = connection.CreateModel();
                channel.QueueDeclare("sensor4", durable: false, exclusive: false, autoDelete: false, arguments: null);
                var sensor = new Sensor4("tmp", channel, value, value, 1, 0);
                sensor.GenerateOne(value);
            }
            else
            {
                throw new InvalidEnumArgumentException("env variable not in range");
            }
            
            connection.Close();
        }


        private static async Task GenerateManyMode(IConnection connection)
        {
            var channel1 = connection.CreateModel();
            channel1.QueueDeclare(
                "sensor1",
                false,
                false,
                false,
                null
            );

            var channel2 = connection.CreateModel();
            channel2.QueueDeclare(
                "sensor2",
                false,
                false,
                false,
                null
            );

            var channel3 = connection.CreateModel();
            channel3.QueueDeclare(
                "sensor3",
                false,
                false,
                false,
                null
            );

            var channel4 = connection.CreateModel();
            channel4.QueueDeclare(
                "sensor4",
                false,
                false,
                false,
                null
            );

            var n1 = int.Parse(Environment.GetEnvironmentVariable("n1") ?? throw new InvalidOperationException("n1"));
            var l1 = int.Parse(Environment.GetEnvironmentVariable("l1") ?? throw new InvalidOperationException("l1"));
            var h1 = int.Parse(Environment.GetEnvironmentVariable("h1") ?? throw new InvalidOperationException("h1"));
            var sleep1 =
                int.Parse(Environment.GetEnvironmentVariable("sleep1") ??
                          throw new InvalidOperationException("sleep1"));

            var n2 = int.Parse(Environment.GetEnvironmentVariable("n2") ?? throw new InvalidOperationException("n2"));
            var l2 = int.Parse(Environment.GetEnvironmentVariable("l2") ?? throw new InvalidOperationException("l2"));
            var h2 = int.Parse(Environment.GetEnvironmentVariable("h2") ?? throw new InvalidOperationException("h2"));
            var sleep2 =
                int.Parse(Environment.GetEnvironmentVariable("sleep2") ??
                          throw new InvalidOperationException("sleep2"));

            var n3 = int.Parse(Environment.GetEnvironmentVariable("n3") ?? throw new InvalidOperationException("n3"));
            var l3 = int.Parse(Environment.GetEnvironmentVariable("l3") ?? throw new InvalidOperationException("l3"));
            var h3 = int.Parse(Environment.GetEnvironmentVariable("h3") ?? throw new InvalidOperationException("h3"));
            var sleep3 =
                int.Parse(Environment.GetEnvironmentVariable("sleep3") ??
                          throw new InvalidOperationException("sleep3"));

            var n4 = int.Parse(Environment.GetEnvironmentVariable("n4") ?? throw new InvalidOperationException("n4"));
            var l4 = int.Parse(Environment.GetEnvironmentVariable("l4") ?? throw new InvalidOperationException("l4"));
            var h4 = int.Parse(Environment.GetEnvironmentVariable("h4") ?? throw new InvalidOperationException("h4"));
            var sleep4 =
                int.Parse(Environment.GetEnvironmentVariable("sleep4") ??
                          throw new InvalidOperationException("sleep4"));


            var sensors = new List<ISensor>();
            for (var i = 0; i < 8; i++)
            {
                sensors.Add(new Sensor1($"1_{i + 1}", channel1, l1, h1, n1, sleep1));
                sensors.Add(new Sensor2($"2_{i + 1}", channel2, l2, h2, n2, sleep2));
                sensors.Add(new Sensor3($"3_{i + 1}", channel3, l3, h3, n3, sleep3));
                sensors.Add(new Sensor4($"4_{i + 1}", channel4, l4, h4, n4, sleep4));
            }

            var tasks = sensors.Select(x => Task.Run(x.GenerateMany)).ToList();

            await Task.WhenAll(tasks);

            channel1.Close();
            channel2.Close();
            channel3.Close();
            channel4.Close();
        }
    }
}
