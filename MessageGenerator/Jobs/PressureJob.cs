using System;
using System.Threading.Tasks;
using MassTransit;
using MessageGenerator.MessageBodies;
using Microsoft.Extensions.Logging;
using Quartz;

namespace MessageGenerator.Jobs
{
    public class PressureJob : IJob
    {
        private readonly ILogger<PressureJob> _logger;
        private readonly IBusControl _busControl;
        private readonly PressureValueSettings _settings;
        public PressureJob(ILogger<PressureJob> logger, IBusControl busControl, PressureValueSettings settings)
        {
            _logger = logger;
            _busControl = busControl;
            _settings = settings;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var sensorId = dataMap.GetIntValue("sensorId");
            var rnd = new Random();

            var sendToUri = new Uri("queue:pressure_data");
            var endPoint = await _busControl.GetSendEndpoint(sendToUri);
            var value = (decimal) rnd.NextDouble() * (_settings.To - _settings.From) + _settings.From;
            var body = new Pressure(sensorId, value);

            _logger.LogInformation("Sending a message: " + body);
            await endPoint.Send(body);
        }
    }
}