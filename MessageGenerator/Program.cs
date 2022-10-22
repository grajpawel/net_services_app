using System;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                        x.AddBus(_ => Bus.Factory.CreateUsingRabbitMq(config =>
                        {
                            config.Host(new Uri($"{RabbitMqConsts.RabbitMqUri}"), h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });
                        }));
                    });
                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory();

                        IConfiguration configuration = hostContext.Configuration;

                        // Create a "key" for the job
                        var jobKey = new JobKey("SendMessageJob");

                        // Register the job with the DI container
                        q.AddJob<SendMessageJob>(opts => opts.WithIdentity(jobKey));

                        // Create a trigger for the job
                        q.AddTrigger(opts => opts
                            .ForJob(jobKey) // link to the HelloWorldJob
                            .WithIdentity("SendMessageJob-trigger") // give the trigger a unique name
                            .WithCronSchedule("0/5 * * * * ?")); // run every 5 seconds

                    });
                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                });
        }
    }
}