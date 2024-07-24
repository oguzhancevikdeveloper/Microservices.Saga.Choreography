using MassTransit;
using MongoDB.Driver;
using SagaChore.Shared.Events;
using SagaChore.StockAPI.Models;
using SagaChore.StockAPI.Services;

namespace SagaChore.StockAPI.Consumers;

public class PaymentFailedEventConsumer(MongoDBService _mongoDBService) : IConsumer<PaymentFailedEvent>
{
    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {

        foreach (var orderItem in context.Message.OrderItems)
        {
            var stock = await (await _mongoDBService.GetCollection<Stock>().FindAsync(x => x.ProductId == orderItem.ProductId.ToString())).FirstOrDefaultAsync();

            if (stock is not null) 
            {
                stock.Count += orderItem.Count;
                await _mongoDBService.GetCollection<Stock>().FindOneAndReplaceAsync(x => x.ProductId == orderItem.ProductId.ToString(), stock);
            } 

        }
    }
}
