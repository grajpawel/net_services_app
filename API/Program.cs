using System;
using API.Consumers;
using API.MongoDb;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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

                    var mongoDbHumiditySettings = configuration.GetSection("MongoDbHumiditySettings").Get<MongoDbSettings>();
                    var mongoDbPressureSettings = configuration.GetSection("MongoDbPressureSettings").Get<MongoDbSettings>();
                    var mongoDbTemperatureSettings = configuration.GetSection("MongoDbTemperatureSettings").Get<MongoDbSettings>();
                    var mongoDbWindSettings = configuration.GetSection("MongoDbWindSettings").Get<MongoDbSettings>();

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
                                e => { e.Consumer(() => new HumidityConsumer(mongoDbHumiditySettings)); });
                            config.ReceiveEndpoint("pressure_data",
                                e => { e.Consumer(() => new PressureConsumer(mongoDbPressureSettings)); });
                            config.ReceiveEndpoint("temperature_data",
                                e => { e.Consumer(() => new TemperatureConsumer(mongoDbTemperatureSettings)); });
                            config.ReceiveEndpoint("wind_data",
                                e => { e.Consumer(() => new WindConsumer(mongoDbWindSettings)); });
                        }));
                    });
                });
        }
    }
}