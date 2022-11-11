using System;
using System.Threading.Tasks;
using API.MongoDb;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API.Consumers
{
    public class WindConsumer : IConsumer<Wind>
    {
        private readonly WindService _service;

        public WindConsumer(WindService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<Wind> context)
        {

            await _service.InsertOneAsync(context.Message);

            await Console.Out.WriteLineAsync(
                $"Wind data received: {context.Message}");
        }
    }
}