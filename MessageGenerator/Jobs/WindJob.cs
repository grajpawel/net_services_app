using System;
using System.Threading.Tasks;
using MassTransit;
using MessageGenerator.MessageBodies;
using Microsoft.Extensions.Logging;
using Quartz;

namespace MessageGenerator.Jobs
{
    public class WindJob : IJob
    {
        private readonly ILogger<WindJob> _logger;
        private readonly IBusControl _busControl;
        private readonly WindValueSettings _settings;
        public WindJob(ILogger<WindJob> logger, IBusControl busControl, WindValueSettings settings)
        {
            _logger = logger;
            this._busControl = busControl;
            _settings = settings;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var sensorId = dataMap.GetIntValue("sensorId");
            var rnd = new Random();

            var sendToUri = new Uri("queue:wind_data");
            var endPoint = await _busControl.GetSendEndpoint(sendToUri);

            var value = (decimal) rnd.NextDouble() * (_settings.To - _settings.From) + _settings.From;
            var direction = (decimal) rnd.NextDouble() * (_settings.DegreeTo - _settings.DegreeFrom) + _settings.DegreeFrom;
            var body = new Wind(sensorId, value, direction);

            _logger.LogInformation("Sending a message: " + body);
            await endPoint.Send(body);
        }
    }
}