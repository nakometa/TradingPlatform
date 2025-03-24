using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models.DTOs;
using Portfolio.Api.Services.Interfaces;
using Shared.Contracts;

namespace Portfolio.Api.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly PortfolioDbContext _context;
        private readonly IPriceCache _priceCache;
        private readonly ILogger<PortfolioService> _logger;

        public PortfolioService(PortfolioDbContext context, IPriceCache priceCache, ILogger<PortfolioService> logger)
        {
            _context = context;
            _priceCache = priceCache;
            _logger = logger;
        }

        public async Task<(decimal TotalValue, List<UserStockDto> Stocks)> GetUserPortfolioAsync(int userId)
        {
            var stocks = await _context.UserStocks
                .Include(us => us.StockCatalog) // Optional ако имаш навигационно пропърти
                .Where(us => us.UserId == userId)
                .ToListAsync();

            List<UserStockDto> result = new();
            decimal totalValue = 0;

            foreach (var stock in stocks)
            {
                if (_priceCache.TryGetPrice(stock.StockCatalog.Ticker, out decimal currentPrice))
                {
                    var dto = new UserStockDto
                    {
                        Ticker = stock.StockCatalog.Ticker,
                        Quantity = stock.Quantity,
                        PricePerUnit = currentPrice
                    };

                    totalValue += dto.Total;
                    result.Add(dto);
                }
                else
                {
                    _logger.LogWarning($"No price for {stock.StockCatalog.Ticker}");
                }
            }

            return (totalValue, result);
        }
    }
}
