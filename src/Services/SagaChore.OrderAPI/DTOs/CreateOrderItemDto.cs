namespace SagaChore.OrderAPI.DTOs;

public record CreateOrderItemDto(string ProductId, int Count, decimal Price);

