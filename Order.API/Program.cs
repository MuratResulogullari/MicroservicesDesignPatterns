using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;

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
    configure.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["MessageBusConfiguration:Host"], h =>
        {
            h.Username(builder.Configuration["MessageBusConfiguration:Username"]);
            h.Password(builder.Configuration["MessageBusConfiguration:Password"]);
        });
    });
});
builder.Services.AddMassTransitHostedService();
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