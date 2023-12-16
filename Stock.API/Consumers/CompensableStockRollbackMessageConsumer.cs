using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Messages;
using Stock.API.Models;
using System.Runtime.InteropServices;

namespace Stock.API.Consumers
{
    public class CompensableStockRollbackMessageConsumer : IConsumer<CompensableStockRollbackMessage>
    {
        private readonly ILogger<OrderCreatedEventConsumer> _logger;
        private readonly StockDbContext _stockDbContext;

        public CompensableStockRollbackMessageConsumer(ILogger<OrderCreatedEventConsumer> logger, StockDbContext stockDbContext)
        {
            _logger = logger;
            _stockDbContext = stockDbContext;
        }

        public async Task Consume(ConsumeContext<CompensableStockRollbackMessage> context)
        {
            if (context.Message.OrderItems.Any())
            {
                foreach (var item in context.Message.OrderItems)
                {
                    var stock = await _stockDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                    if (stock != null)
                    {
                        stock.Count += item.Count;
                    }
                }
                await _stockDbContext.SaveChangesAsync();
                _logger.LogInformation("Stok was released. ");
            }
        }
    }
}