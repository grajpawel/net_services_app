using System;
using System.Threading.Tasks;
using API.MongoDb;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API.Consumers
{
    public class TemperatureConsumer : IConsumer<Temperature>

    {
        private MongoDbSettings _dbSettings;

        public TemperatureConsumer(MongoDbSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        public async Task Consume(ConsumeContext<Temperature> context)
        {
            var service = new TemperatureService(_dbSettings);

            await service.InsertOneAsync(context.Message);

            await Console.Out.WriteLineAsync(
                $"Temperature data received: {context.Message}");
        }
    }
}