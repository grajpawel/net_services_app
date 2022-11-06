using System;
using System.Threading.Tasks;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API.Consumers
{
    public class WindConsumer : IConsumer<Wind>

    {
        public async Task Consume(ConsumeContext<Wind> context)
        {
            await Console.Out.WriteLineAsync(
                $"Wind data received: {context.Message}");
        }
    }
}