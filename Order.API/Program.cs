using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Models;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectionString"));
});

builder.Services.AddMassTransit(configure =>
{

    configure.AddConsumer<OrderCompletedConsumer>();
    configure.AddConsumer<OrderFailedConsumer>();
    configure.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["MessageBusConfiguration:Host"], h =>
        {
            h.Username(builder.Configuration["MessageBusConfiguration:Username"]);
            h.Password(builder.Configuration["MessageBusConfiguration:Password"]);
        }); 
        cfg.ReceiveEndpoint(RabbitMQConsts.OrchestrationOrderCompletedQueueName, configureEndpoint =>
        {
            configureEndpoint.ConfigureConsumer<OrderCompletedConsumer>(context);
        });
        cfg.ReceiveEndpoint(RabbitMQConsts.OrchestrationOrderFailedQueueName, configureEndpoint =>
        {
            configureEndpoint.ConfigureConsumer<OrderFailedConsumer>(context);
        });
    });
});
//builder.Services.AddMassTransitHostedService();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
// Configure MassTransit consumers, if any

app.Run();