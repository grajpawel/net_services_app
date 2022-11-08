using System;
using System.Threading.Tasks;
using API.MongoDb;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API.Consumers
{
    public class HumidityConsumer : IConsumer<Humidity>

    {
        private MongoDbSettings _dbSettings;

        public HumidityConsumer(MongoDbSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        public async Task Consume(ConsumeContext<Humidity> context)
        {
            var service = new HumidityService(_dbSettings);

            await service.InsertOneAsync(context.Message);

            await Console.Out.WriteLineAsync(
                $"Humidity data received: {context.Message}");
        }
    }
}