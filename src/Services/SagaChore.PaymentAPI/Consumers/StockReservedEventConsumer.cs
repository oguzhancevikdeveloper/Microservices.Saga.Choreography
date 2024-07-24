using MassTransit;
using SagaChore.Shared.Events;

namespace SagaChore.PaymentAPI.Consumers;

public class StockReservedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<StockReservedEvent>
{
    public async Task Consume(ConsumeContext<StockReservedEvent> context)
    {
        Random random = new Random();
        int number = random.Next(0, 100);

        if (number % 2 == 0)
        {
            PaymentCompletedEvent paymentCompletedEvent = new()
            {
                OrderId = context.Message.OrderId
            };
            await publishEndpoint.Publish(paymentCompletedEvent);
            await Console.Out.WriteLineAsync("Ödeme başarılı...");
        }
        else
        {
            PaymentFailedEvent paymentFailedEvent = new()
            {
                OrderId = context.Message.OrderId,
                Message = "Yetersiz bakiye...",
                OrderItems = context.Message.OrderItemMessages
            };
            await publishEndpoint.Publish(paymentFailedEvent);
            await Console.Out.WriteLineAsync("Ödeme başarısız...");
        }
    }
}
