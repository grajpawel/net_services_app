using System;
using MassTransit;
using MessageGenerator.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace MessageGenerator
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
                    var humidityValueSettings = configuration.GetSection(nameof(HumidityValueSettings)).Get<HumidityValueSettings>();
                    var pressureValueSettings = configuration.GetSection(nameof(PressureValueSettings)).Get<PressureValueSettings>();
                    var temperatureValueSettings = configuration.GetSection(nameof(TemperatureValueSettings)).Get<TemperatureValueSettings>();
                    var windValueSettings = configuration.GetSection(nameof(WindValueSettings)).Get<WindValueSettings>();

                    services.AddSingleton(humidityValueSettings);
                    services.AddSingleton(pressureValueSettings);
                    services.AddSingleton(temperatureValueSettings);
                    services.AddSingleton(windValueSettings);

                    services.AddMassTransit(x =>
                    {
                        x.AddBus(_ => Bus.Factory.CreateUsingRabbitMq(config =>
                        {
                            config.Host(new Uri($"{appOptions.RabbitMqUri}"), h =>
                            {
                                h.Username(appOptions.Username);
                                h.Password(appOptions.Password);
                            });
                        }));
                    });
                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory();

                        var humidityJobKey = new JobKey("HumidityJob");
                        var pressureJobKey = new JobKey("PressureJob");
                        var temperatureJobKey = new JobKey("TemperatureJob");
                        var windJobKey = new JobKey("WindJob");

                        for (var i = 0; i < 10; i++)
                        {
                            var counter = i;
                            q.AddJob<HumidityJob>(opts => opts
                                .WithIdentity(humidityJobKey+counter.ToString())
                                .UsingJobData("sensorId", counter));

                            q.AddJob<PressureJob>(opts => opts
                                .WithIdentity(pressureJobKey+counter.ToString())
                                .UsingJobData("sensorId", counter));

                            q.AddJob<TemperatureJob>(opts => opts
                                .WithIdentity(temperatureJobKey+counter.ToString())
                                .UsingJobData("sensorId", counter));

                            q.AddJob<WindJob>(opts => opts
                                .WithIdentity(windJobKey+counter.ToString())
                                .UsingJobData("sensorId", counter));

                            q.AddTrigger(opts => opts
                                .ForJob(humidityJobKey+counter.ToString())
                                .WithIdentity("humidityJobKey-trigger"+counter.ToString())
                                .WithCronSchedule(appOptions.HumidityJobCron));

                            q.AddTrigger(opts => opts
                                .ForJob(pressureJobKey+counter.ToString())
                                .WithIdentity("pressureJobKey-trigger"+counter.ToString())
                                .WithCronSchedule(appOptions.PressureJobCron));

                            q.AddTrigger(opts => opts
                                .ForJob(temperatureJobKey+counter.ToString())
                                .WithIdentity("temperatureJobKey-trigger"+counter.ToString())
                                .WithCronSchedule(appOptions.TemperatureJobCron));

                            q.AddTrigger(opts => opts
                                .ForJob(windJobKey+counter.ToString())
                                .WithIdentity("windJobKey-trigger"+counter.ToString())
                                .WithCronSchedule(appOptions.WindJobCron));
                        }
                    });
                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                });
        }
    }
}