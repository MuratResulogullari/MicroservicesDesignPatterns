using MassTransit;
using Microsoft.EntityFrameworkCore;
using Stock.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StockDbContext>(options =>
{
    options.UseInMemoryDatabase("StockDb");
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
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StockDbContext>();
    context.Stocks.Add(new Stock.API.Models.Stock("2E773231-D664-45CA-92EC-6432ED515CBB", 100));
    context.Stocks.Add(new Stock.API.Models.Stock("A1041B27-DFA9-411F-BE39-4EAE3D6C499A", 1203));
    context.Stocks.Add(new Stock.API.Models.Stock("A5FC7507-6AA5-4603-82E6-5759A452339F", 345));
    context.Stocks.Add(new Stock.API.Models.Stock("DD6659C5-AAE8-4A06-A338-BF3EF5F88A94", 28));
    context.SaveChanges();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();