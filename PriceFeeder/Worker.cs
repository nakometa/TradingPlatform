using MassTransit;
using Shared.Contracts;

namespace PriceFeeder
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly Random _random = new();
        private readonly Dictionary<string, decimal> _tickerPrices = new()
        {
            { "AAPL", 150 },
            { "TSLA", 700 },
            { "NVDA", 250 },
            { "AMZN", 3300 },
            { "GOOGL", 2800 }
        };

        public Worker(ILogger<Worker> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<PriceUpdate> priceUpdates = new List<PriceUpdate>();

                foreach (var ticker in _tickerPrices.Keys.ToList())
                {
                    var oldPrice = _tickerPrices[ticker];
                    var newPrice = ChangePriceRandomly(oldPrice);
                    _tickerPrices[ticker] = newPrice;

                    var priceUpdate = new PriceUpdate
                    {
                        Ticker = ticker,
                        Price = newPrice,
                        UpdatedAt = DateTime.UtcNow
                    };

                    priceUpdates.Add(priceUpdate);
                }

                var batchUpdate = new PriceBatchUpdate
                {
                    Prices = priceUpdates,
                    UpdatedAt = DateTime.UtcNow
                };

                await _publishEndpoint.Publish(batchUpdate, stoppingToken);

                _logger.LogInformation($"Published batch price update: {string.Join(", ", priceUpdates.Select(p => $"{p.Ticker} - {p.Price}"))}");

                await Task.Delay(1000, stoppingToken);
            }
        }

        private decimal ChangePriceRandomly(decimal price)
        {
            const decimal maxChangePercent = 0.02m;
            var changePercent = (decimal)(_random.NextDouble() * (double)(maxChangePercent * 2)) - maxChangePercent;
            var newPrice = price * (1 + changePercent);

            return Math.Max(1, Math.Round(newPrice, 2));
        }
    }
}
