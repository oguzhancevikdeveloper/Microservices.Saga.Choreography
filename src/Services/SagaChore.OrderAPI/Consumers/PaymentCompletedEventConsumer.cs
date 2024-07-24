using MassTransit;
using SagaChore.OrderAPI.Models.Contexts;
using SagaChore.OrderAPI.Models;
using SagaChore.Shared.Events;
using Microsoft.EntityFrameworkCore;

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