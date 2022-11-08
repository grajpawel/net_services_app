using System;
using System.Threading.Tasks;
using API.MongoDb;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API.Consumers
{
    public class WindConsumer : IConsumer<Wind>

    {
        private MongoDbSettings _dbSettings;

        public WindConsumer(MongoDbSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        public async Task Consume(ConsumeContext<Wind> context)
        {
            var service = new WindService(_dbSettings);

            await service.InsertOneAsync(context.Message);

            await Console.Out.WriteLineAsync(
                $"Wind data received: {context.Message}");
        }
    }
}