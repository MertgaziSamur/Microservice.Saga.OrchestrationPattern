﻿using MassTransit;
using Order.API.Context;
using Shared.OrderEvents;

namespace Order.API.Consumers
{
    public class OrderCompletedEventConsumer(OrderAPIDbContext orderDbContext) : IConsumer<OrderCompletedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
        {
            var order = await orderDbContext.Orders.FindAsync(context.Message.OrderId);

            if (order != null)
            {
                order.OrderStatus = Enums.OrderStatus.Completed;
                await orderDbContext.SaveChangesAsync();
            }
        }
    }
}
