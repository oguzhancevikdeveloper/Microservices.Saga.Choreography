using SagaChore.Shared.Messages;

namespace SagaChore.Shared.Events;

public class PaymentFailedEvent
{
    public Guid OrderId { get; set; }
    public string Message { get; set; } = default!;
    public List<OrderItemMessage> OrderItems { get; set; } = default!;
}
