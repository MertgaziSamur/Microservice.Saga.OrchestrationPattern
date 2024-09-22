using MassTransit;
using Stock.API.Services;
using MongoDB.Driver;
using Stock.API.Consumers;
using Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configuratior =>
{
    configuratior.AddConsumer<OrderCreatedEventConsumer>();
    configuratior.AddConsumer<StockRollbackMessageConsumer>();
    configuratior.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
        _configure.ReceiveEndpoint(RabbitMQSettings.Stock_RollbackMessageQueue, e => e.ConfigureConsumer<StockRollbackMessageConsumer>(context));
    });
});

builder.Services.AddSingleton<MongoDbService>();

var app = builder.Build();

using var scope = builder.Services.BuildServiceProvider().CreateScope();
var mongoDbService = scope.ServiceProvider.GetRequiredService<MongoDbService>();

if (!await (await mongoDbService.GetCollection<Stock.API.Models.Stock>().FindAsync(x => true)).AnyAsync())
{
    await mongoDbService.GetCollection<Stock.API.Models.Stock>().InsertOneAsync(new Stock.API.Models.Stock() { ProductId = 1, Count = 200 });
    await mongoDbService.GetCollection<Stock.API.Models.Stock>().InsertOneAsync(new Stock.API.Models.Stock() { ProductId = 2, Count = 300 });
    await mongoDbService.GetCollection<Stock.API.Models.Stock>().InsertOneAsync(new Stock.API.Models.Stock() { ProductId = 3, Count = 50 });
    await mongoDbService.GetCollection<Stock.API.Models.Stock>().InsertOneAsync(new Stock.API.Models.Stock() { ProductId = 4, Count = 20 });
    await mongoDbService.GetCollection<Stock.API.Models.Stock>().InsertOneAsync(new Stock.API.Models.Stock() { ProductId = 5, Count = 60 });
}

app.Run();
