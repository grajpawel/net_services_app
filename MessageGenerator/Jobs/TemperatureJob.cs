using System;
using System.Threading.Tasks;
using MassTransit;
using MessageGenerator.MessageBodies;
using Microsoft.Extensions.Logging;
using Quartz;

namespace MessageGenerator.Jobs
{
    public class  TemperatureJob : IJob
    {
        private readonly ILogger<TemperatureJob> _logger;
        private readonly IBusControl _busControl;
        public TemperatureJob(ILogger<TemperatureJob> logger, IBusControl busControl)
        {
            _logger = logger;
            this._busControl = busControl;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var sensorId = dataMap.GetIntValue("sensorId");

            var sendToUri = new Uri("queue:temperature_data");
            var endPoint = await _busControl.GetSendEndpoint(sendToUri);
            var body = new Temperature(sensorId, 10);
            _logger.LogInformation("Sending a message: " + body);
            await endPoint.Send(body);
        }
    }
}