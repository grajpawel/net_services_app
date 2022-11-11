using System;
using System.Threading.Tasks;
using API.MongoDb;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API.Consumers
{
    public class TemperatureConsumer : IConsumer<Temperature>
    {
        private readonly TemperatureService _service;

        public TemperatureConsumer(TemperatureService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<Temperature> context)
        {

            await _service.InsertOneAsync(context.Message);

            await Console.Out.WriteLineAsync(
                $"Temperature data received: {context.Message}");
        }
    }
}