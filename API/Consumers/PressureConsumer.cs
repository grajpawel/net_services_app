using System;
using System.Threading.Tasks;
using API.MongoDb;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API.Consumers
{
    public class PressureConsumer : IConsumer<Pressure>
    {
        private readonly PressureService _service;

        public PressureConsumer(PressureService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<Pressure> context)
        {

            await _service.InsertOneAsync(context.Message);

            await Console.Out.WriteLineAsync(
                $"Pressure data received: {context.Message}");
        }
    }
}