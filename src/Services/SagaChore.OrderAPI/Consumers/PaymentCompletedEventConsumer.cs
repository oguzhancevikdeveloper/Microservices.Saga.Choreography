using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaChore.OrderAPI.Models;
using SagaChore.OrderAPI.Models.Contexts;
using SagaChore.Shared.Events;

namespace SagaChore.OrderAPI.Consumers;

public class PaymentCompletedEventConsumer(OrderAPIDbContext _orderAPIDbContext) : IConsumer<PaymentCompletedEvent>
{
    public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    {
        Order? order = await _orderAPIDbContext.Orders.SingleOrDefaultAsync(x => x.Id.Equals(context.Message.OrderId));
        if (order != null) order.OrderStatus = Enums.OrderStatus.Completed;
        await _orderAPIDbContext.SaveChangesAsync();
    }
}
