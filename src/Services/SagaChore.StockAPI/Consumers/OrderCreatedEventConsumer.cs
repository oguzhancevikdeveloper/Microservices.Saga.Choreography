using MassTransit;
using MongoDB.Driver;
using SagaChore.Shared;
using SagaChore.Shared.Events;
using SagaChore.StockAPI.Models;
using SagaChore.StockAPI.Services;

namespace SagaChore.StockAPI.Consumers;

public class OrderCreatedEventConsumer(MongoDBService _mongoDBService, IPublishEndpoint _publishEndpoint, ISendEndpointProvider _sendEndpointProvider) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {

        List<bool> stockResult = new List<bool>();

        IMongoCollection<Stock> collection = _mongoDBService.GetCollection<Stock>();

        foreach (var orderItem in context.Message.OrderItems)
        {
            stockResult.Add(await (await collection.FindAsync(x => x.ProductId == orderItem.ProductId.ToString() && x.Count > (long)orderItem.Count)).AnyAsync());
        }

        if (stockResult.TrueForAll(x => x.Equals(true)))
        {
            foreach (var orderItem in context.Message.OrderItems)
            {
                Stock stock = await (await collection.FindAsync(x => x.ProductId == orderItem.ProductId.ToString())).FirstOrDefaultAsync();

                stock.Count -= orderItem.Count;

                await collection.FindOneAndReplaceAsync(x => x.ProductId == orderItem.ProductId.ToString(), stock);
            }
            var sendPoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue: {RabbitMQSettings.Payment_StockReservedEventQueue}"));

            StockReservedEvent stockReservedEvent = new()
            {
                BuyerId = context.Message.BuyerId,
                OrderId = context.Message.OrderId,
                OrderItemMessages = context.Message.OrderItems,
                TotalPrice = context.Message.TotalPrice
            };

            await sendPoint.Send(stockReservedEvent);
        }

        else
        {
            StockNotReservedEvent stockNotReservedEvent = new()
            {
                BuyerId = context.Message.BuyerId,
                Message = "Stock miktarı yetersiz",
                OrderId = context.Message.OrderId
            };
            await _publishEndpoint.Publish(stockNotReservedEvent);
          
        }
    }
}
