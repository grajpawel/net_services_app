﻿using System;
using System.Threading.Tasks;
using MassTransit;
using MessageGenerator.MessageBodies;
using Microsoft.Extensions.Logging;
using Quartz;

namespace MessageGenerator.Jobs
{
    public class HumidityJob : IJob
    {
        private readonly ILogger<HumidityJob> _logger;
        private readonly IBusControl _busControl;
        public HumidityJob(ILogger<HumidityJob> logger, IBusControl busControl)
        {
            _logger = logger;
            this._busControl = busControl;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var sendToUri = new Uri("queue:humidity_data");
            var endPoint = await _busControl.GetSendEndpoint(sendToUri);
            var body = new Humidity(1, 10);
            _logger.LogInformation("Sending a message: " + body);
            await endPoint.Send(body);
        }
    }
}