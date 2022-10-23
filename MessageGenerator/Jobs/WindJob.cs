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
        public WindJob(ILogger<WindJob> logger, IBusControl busControl)
        {
            _logger = logger;
            this._busControl = busControl;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var sendToUri = new Uri("queue:wind_data");
            var endPoint = await _busControl.GetSendEndpoint(sendToUri);
            var body = new Wind(1, 10, "NorthWest");
            _logger.LogInformation("Sending a message: " + body);
            await endPoint.Send(body);
        }
    }
}