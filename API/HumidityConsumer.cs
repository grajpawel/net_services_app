using System;
using System.Threading.Tasks;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API
{
    public class HumidityConsumer : IConsumer<Humidity>

    {
        public async Task Consume(ConsumeContext<Humidity> context)
        {
            await Console.Out.WriteLineAsync(
                $"Humidity data Received: {context.Message}");
        }
    }
}