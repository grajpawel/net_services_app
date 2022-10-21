using System;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace MessageGenerator
{
    public class Program
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
                    services.AddMassTransit(x =>
                    {
                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                        {
                            config.Host(new Uri("amqp://guest:guest@net.services.rabbitmq"), h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });
                        }));
                    });
                    services.AddQuartz(q =>
                    {
                        IConfiguration configuration = hostContext.Configuration;

                        q.UseMicrosoftDependencyInjectionScopedJobFactory();

                        // Create a "key" for the job
                        var jobKey = new JobKey("HelloWorldJob");

                        // Register the job with the DI container
                        q.AddJob<HelloWorldJob>(opts => opts.WithIdentity(jobKey));

                        // Create a trigger for the job
                        q.AddTrigger(opts => opts
                            .ForJob(jobKey) // link to the HelloWorldJob
                            .WithIdentity("HelloWorldJob-trigger") // give the trigger a unique name
                            .WithCronSchedule("0/5 * * * * ?")); // run every 5 seconds

                    });
                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                });
        }
    }
}