using System;
using System.Threading.Tasks;
using API.MongoDb;
using MassTransit;
using MessageGenerator.MessageBodies;

namespace API.Consumers
{
    public class PressureConsumer : IConsumer<Pressure>

    {
        private MongoDbSettings _dbSettings;

        public PressureConsumer(MongoDbSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        public async Task Consume(ConsumeContext<Pressure> context)
        {
            var service = new PressureService(_dbSettings);

            await service.InsertOneAsync(context.Message);

            await Console.Out.WriteLineAsync(
                $"Pressure data received: {context.Message}");
        }
    }
}