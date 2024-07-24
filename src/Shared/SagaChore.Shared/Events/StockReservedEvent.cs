using SagaChore.Shared.Messages;

namespace SagaChore.Shared.Events;

public class StockReservedEvent
{
    public Guid BuyerId { get; set; }
    public Guid OrderId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemMessage> OrderItemMessages { get; set; } = default!;
}