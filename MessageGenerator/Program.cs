using System;
using MassTransit;
using MessageGenerator.Jobs;
using Microsoft.Extensions.Configuration;
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

                        // Create a "key" for jobs
                        var humidityJobKey = new JobKey("HumidityJob");
                        var pressureJobKey = new JobKey("PressureJob");
                        var temperatureJobKey = new JobKey("TemperatureJob");
                        var windJobKey = new JobKey("WindJob");

                        // Register the job with the DI container
                        q.AddJob<HumidityJob>(opts => opts.WithIdentity(humidityJobKey));
                        q.AddJob<PressureJob>(opts => opts.WithIdentity(pressureJobKey));
                        q.AddJob<TemperatureJob>(opts => opts.WithIdentity(temperatureJobKey));
                        q.AddJob<WindJob>(opts => opts.WithIdentity(windJobKey));

                        // Create a trigger for the job
                        q.AddTrigger(opts => opts
                            .ForJob(humidityJobKey)
                            .WithIdentity("humidityJobKey-trigger")
                            .WithCronSchedule(appOptions.HumidityJobCron));

                        // Create a trigger for the job
                        q.AddTrigger(opts => opts
                            .ForJob(pressureJobKey)
                            .WithIdentity("pressureJobKey-trigger")
                            .WithCronSchedule(appOptions.PressureJobCron));

                        // Create a trigger for the job
                        q.AddTrigger(opts => opts
                            .ForJob(temperatureJobKey)
                            .WithIdentity("temperatureJobKey-trigger")
                            .WithCronSchedule(appOptions.TemperatureJobCron));

                        // Create a trigger for the job
                        q.AddTrigger(opts => opts
                            .ForJob(windJobKey)
                            .WithIdentity("windJobKey-trigger")
                            .WithCronSchedule(appOptions.WindJobCron));
                    });
                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                });
        }
    }
}