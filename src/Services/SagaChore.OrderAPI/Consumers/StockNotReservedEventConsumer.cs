using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaChore.OrderAPI.Models;
using SagaChore.OrderAPI.Models.Contexts;
using SagaChore.Shared.Events;

namespace SagaChore.OrderAPI.Consumers;

public class StockNotReservedEventConsumer(OrderAPIDbContext _orderAPIDbContext) : IConsumer<StockNotReservedEvent>
{
    public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
    {
        Order? order = await _orderAPIDbContext.Orders.SingleOrDefaultAsync(x => x.Id.Equals(context.Message.OrderId));

        if (order != null) order.OrderStatus = Enums.OrderStatus.Fail;

        await _orderAPIDbContext.SaveChangesAsync();

    }
}
