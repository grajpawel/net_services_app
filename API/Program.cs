using System;
using API.Consumers;
using API.Dtos;
using API.MongoDb;
using API.Helpers;
using AutoMapper;
using MassTransit;
using MessageGenerator.MessageBodies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                    var mapperConfiguration = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<HumidityDto, Humidity>();
                        cfg.CreateMap<Humidity, HumidityDto>();
                        cfg.CreateMap<PressureDto, Pressure>();
                        cfg.CreateMap<Pressure, PressureDto>();
                        cfg.CreateMap<TemperatureDto, Temperature>();
                        cfg.CreateMap<Temperature, TemperatureDto>();
                        cfg.CreateMap<WindDto, Wind>();
                        cfg.CreateMap<Wind, WindDto>();
                    });

                    var mapper = mapperConfiguration.CreateMapper();
                    services.AddSingleton<IMapper>(mapper);

                    var configuration = hostContext.Configuration;
                    var appOptions = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();

                    var mongoDbHumiditySettings =
                        configuration.GetSection("MongoDbHumiditySettings").Get<MongoDbHumiditySettings>();
                    var mongoDbPressureSettings =
                        configuration.GetSection("MongoDbPressureSettings").Get<MongoDbPressureSettings>();
                    var mongoDbTemperatureSettings =
                        configuration.GetSection("MongoDbTemperatureSettings").Get<MongoDbTemperatureSettings>();
                    var mongoDbWindSettings = configuration.GetSection("MongoDbWindSettings").Get<MongoDbWindSettings>();

                    services.AddSingleton(mongoDbHumiditySettings);
                    services.AddSingleton(mongoDbPressureSettings);
                    services.AddSingleton(mongoDbTemperatureSettings);
                    services.AddSingleton(mongoDbWindSettings);

                    ISortHelper<HumidityDto> humiditySortHelper = new SortHelper<HumidityDto>(); 
                    ISortHelper<PressureDto> pressureSortHelper = new SortHelper<PressureDto>(); 
                    ISortHelper<TemperatureDto> temperatureSortHelper = new SortHelper<TemperatureDto>(); 
                    ISortHelper<WindDto> windSortHelper = new SortHelper<WindDto>();

                    services.AddSingleton(humiditySortHelper);
                    services.AddSingleton(pressureSortHelper);
                    services.AddSingleton(temperatureSortHelper);
                    services.AddSingleton(windSortHelper);

                    var humidityService =  new HumidityService(mongoDbHumiditySettings, mapper, humiditySortHelper);
                    var pressureService =  new PressureService(mongoDbPressureSettings, mapper, pressureSortHelper);
                    var temperatureService =  new TemperatureService(mongoDbTemperatureSettings, mapper, temperatureSortHelper);
                    var windService =  new WindService(mongoDbWindSettings, mapper, windSortHelper);

                    services.AddMassTransit(x =>
                    {
                        x.AddBus(_ => Bus.Factory.CreateUsingRabbitMq(rabbitConfig =>
                        {
                            rabbitConfig.Host(new Uri($"{appOptions.RabbitMqUri}"), h =>
                            {
                                h.Username(appOptions.Username);
                                h.Password(appOptions.Password);
                            });

                            rabbitConfig.ReceiveEndpoint("humidity_data",
                                e => { e.Consumer(() => new HumidityConsumer(humidityService)); });
                            rabbitConfig.ReceiveEndpoint("pressure_data",
                                e => { e.Consumer(() => new PressureConsumer(pressureService)); });
                            rabbitConfig.ReceiveEndpoint("temperature_data",
                                e => { e.Consumer(() => new TemperatureConsumer(temperatureService)); });
                            rabbitConfig.ReceiveEndpoint("wind_data",
                                e => { e.Consumer(() => new WindConsumer(windService)); });
                        }));
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}