using System;
using System.Threading.Tasks;
using API.MongoDb;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API.Consumers
{
    public class HumidityConsumer : IConsumer<Humidity>
    {
        private readonly HumidityService _service;

        public HumidityConsumer(HumidityService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<Humidity> context)
        {

            await _service.InsertOneAsync(context.Message);

            await Console.Out.WriteLineAsync(
                $"Humidity data received: {context.Message}");
        }
    }
}