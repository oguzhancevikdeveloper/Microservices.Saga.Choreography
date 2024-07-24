using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaChore.OrderAPI.Models.Contexts;
using SagaChore.Shared.Events;

namespace SagaChore.OrderAPI.Consumers;

public class PaymentFailedEventConsumer(OrderAPIDbContext _orderAPIDbContext) : IConsumer<PaymentFailedEvent>
{
    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        var order = await _orderAPIDbContext.Orders.SingleOrDefaultAsync(x => x.Id.Equals(context.Message.OrderId));
        if (order != null) order.OrderStatus = Enums.OrderStatus.Fail;
        await _orderAPIDbContext.SaveChangesAsync();

    }
}
