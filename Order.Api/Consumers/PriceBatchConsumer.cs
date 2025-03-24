using MassTransit;
using Shared.Contracts;

namespace Order.Api.Consumers
{
    public class PriceBatchUpdateConsumer : IConsumer<PriceBatchUpdate>
    {
        private readonly IPriceCache _priceCache;
        private readonly ILogger<PriceBatchUpdateConsumer> _logger;

        public PriceBatchUpdateConsumer(ILogger<PriceBatchUpdateConsumer> logger, IPriceCache priceCache)
        {
            _logger = logger;
            _priceCache = priceCache;
        }

        public Task Consume(ConsumeContext<PriceBatchUpdate> context)
        {
            var batch = context.Message;

            foreach (var update in batch.Prices)
            {
                _logger.LogInformation($"Received price update: {update.Ticker} - {update.Price}");
                _priceCache.UpdatePrice(update.Ticker, update.Price);
            }

            return Task.CompletedTask;
        }
    }
}
