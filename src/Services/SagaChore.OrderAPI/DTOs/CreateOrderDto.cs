namespace SagaChore.OrderAPI.DTOs;

public record CreateOrderDto(string BuyerId, List<CreateOrderItemDto> CreateOrderItemDto);

