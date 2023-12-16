using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Reflection;
using WorkerService;
using WorkerService.Models;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
            .EntityFrameworkRepository(configure =>
            {
                configure.AddDbContext<DbContext, OrderStateDbContext>((provider, builder) =>
                {
                    builder.UseSqlServer(hostContext.Configuration.GetConnectionString("SqlConnectionString"), migration =>
                    {
                        migration.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    });
                });
            });

            cfg.AddBus(busFactory => Bus.Factory.CreateUsingRabbitMq(configure =>
            {
                configure.Host(hostContext.Configuration["MessageBusConfiguration:Host"], opt =>
                {
                    opt.Username(hostContext.Configuration["MessageBusConfiguration:Username"]);
                    opt.Password(hostContext.Configuration["MessageBusConfiguration:Password"]);
                });
                configure.ReceiveEndpoint(RabbitMQConsts.OrchestrationOrderSagaQueueName, configureEndpoint =>
                {
                    configureEndpoint.ConfigureSaga<OrderStateInstance>(busFactory);
                });
            }));
        });

        services.AddHostedService<Worker>();
    })
    .Build();

try
{
    await host.RunAsync();
}
catch (AggregateException ex)
{
    foreach (var innerException in ex.InnerExceptions)
    {
        // Log or handle each inner exception
        Console.WriteLine(innerException.Message);
    }
}