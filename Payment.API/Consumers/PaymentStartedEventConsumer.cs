using MassTransit;
using Shared.PaymentEvents;
using Shared.Settings;

namespace Payment.API.Consumers
{
    public class PaymentStartedEventConsumer(ISendEndpointProvider sendEndpointProvider) : IConsumer<PaymentStartedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentStartedEvent> context)
        {
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));
            if (true)
            {
                PaymentCompletedEvent paymentCompletedEvent = new PaymentCompletedEvent(context.Message.CorrelationId)
                {

                };

                await sendEndpoint.Send(paymentCompletedEvent);
            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new PaymentFailedEvent(context.Message.CorrelationId)
                {
                    OrderItems = context.Message.OrderItems,
                    Message = "Payment Is Failed!",
                };

                await sendEndpoint.Send(paymentFailedEvent);
            }

        }
    }
}
