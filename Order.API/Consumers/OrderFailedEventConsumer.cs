using MassTransit;
using Order.API.Context;
using Shared.OrderEvents;

namespace Order.API.Consumers
{
    public class OrderFailedEventConsumer(OrderAPIDbContext orderDbContext) : IConsumer<OrderFailedEvent>
    {
        public async Task Consume(ConsumeContext<OrderFailedEvent> context)
        {
            var order = await orderDbContext.Orders.FindAsync(context.Message.OrderId);

            if (order != null)
            {
                order.OrderStatus = Enums.OrderStatus.Failed;
                await orderDbContext.SaveChangesAsync();
            }
        }
    }
}
