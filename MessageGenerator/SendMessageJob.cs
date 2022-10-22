using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace MessageGenerator
{
    public class SendMessageJob : IJob
    {

        private readonly ILogger<SendMessageJob> _logger;
        private readonly IBusControl busControl;
        public SendMessageJob(ILogger<SendMessageJob> logger, IBusControl busControl)
        {
            _logger = logger;
            this.busControl = busControl;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var sendToUri = new Uri($"{RabbitMqConsts.RabbitMqUri}/test");
            var endPoint = await busControl.GetSendEndpoint(sendToUri);
            await endPoint.Send(new RabbitMqConsts());

        }
    }
}