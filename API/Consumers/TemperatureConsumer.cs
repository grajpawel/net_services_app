using System;
using System.Threading.Tasks;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API.Consumers
{
    public class TemperatureConsumer : IConsumer<Temperature>

    {
        public async Task Consume(ConsumeContext<Temperature> context)
        {
            await Console.Out.WriteLineAsync(
                $"Temperature data received: {context.Message}");
        }
    }
}