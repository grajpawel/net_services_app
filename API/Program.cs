using System;
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
                        }));
                    });
                });
        }
    }
}