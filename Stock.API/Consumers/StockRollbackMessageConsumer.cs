using MassTransit;
using MongoDB.Driver;
using Shared.Messages;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class StockRollbackMessageConsumer(MongoDbService mongoDbService) : IConsumer<StockRollbackMessage>
    {
        public async Task Consume(ConsumeContext<StockRollbackMessage> context)
        {
            var stockColection = mongoDbService.GetCollection<Stock.API.Models.Stock>();

            foreach (var orderItem in context.Message.OrderItems)
            {
                var stock = await (await stockColection.FindAsync(x => x.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();

                stock.Count += orderItem.Count;

                await stockColection.FindOneAndReplaceAsync(x => x.ProductId == orderItem.ProductId, stock);
            }
        }
    }
}
