using System;
using API.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    var appOptions = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();

                    services.AddMassTransit(x =>
                    {
                        x.AddBus(_ => Bus.Factory.CreateUsingRabbitMq(config =>
                        {
                            config.Host(new Uri($"{appOptions.RabbitMqUri}"), h =>
                            {
                                h.Username(appOptions.Username);
                                h.Password(appOptions.Password);
                            });

                            config.ReceiveEndpoint("humidity_data",
                                e => { e.Consumer<HumidityConsumer>(); });
                            config.ReceiveEndpoint("pressure_data",
                                e => { e.Consumer<PressureConsumer>(); });
                            config.ReceiveEndpoint("temperature_data",
                                e => { e.Consumer<TemperatureConsumer>(); });
                            config.ReceiveEndpoint("wind_data",
                                e => { e.Consumer<WindConsumer>(); });
                        }));
                    });
                });
        }
    }
}