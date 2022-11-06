using System;
using System.Threading.Tasks;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API.Consumers
{
    public class HumidityConsumer : IConsumer<Humidity>

    {
        public async Task Consume(ConsumeContext<Humidity> context)
        {
            await Console.Out.WriteLineAsync(
                $"Humidity data received: {context.Message}");
        }
    }
}