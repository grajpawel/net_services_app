using System;
using System.Threading.Tasks;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API.Consumers
{
    public class PressureConsumer : IConsumer<Pressure>

    {
        public async Task Consume(ConsumeContext<Pressure> context)
        {
            await Console.Out.WriteLineAsync(
                $"Pressure data received: {context.Message}");
        }
    }
}