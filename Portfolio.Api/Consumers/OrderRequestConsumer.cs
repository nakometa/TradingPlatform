using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Api.Data;
using Portfolio.Api.Models.Entities;
using Shared.Contracts;

namespace Portfolio.Api.Consumers
{
    public class OrderRequestConsumer : IConsumer<OrderRequest>
    {
        private readonly PortfolioDbContext _context;
        private readonly ILogger<OrderRequestConsumer> _logger;

        public OrderRequestConsumer(PortfolioDbContext context, ILogger<OrderRequestConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderRequest> context)
        {
            var request = context.Message;
            _logger.LogInformation($"Received order request: UserId={request.UserId}, Ticker={request.Ticker}, Quantity={request.Quantity}, Side={request.Side}");

            var user = await _context.Users
                .Include(u => u.UserStocks)
                .FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
            {
                user = new User
                {
                    Id = request.UserId,
                    UserStocks = new List<UserStock>()
                };
                _context.Users.Add(user);
                _logger.LogInformation($"Created new user with Id={request.UserId}");
            }

            var stockCatalog = await _context.StockCatalogs.FirstOrDefaultAsync(sc => sc.Ticker == request.Ticker);

            if (stockCatalog == null)
            {
                await context.RespondAsync(new OrderResponse
                {
                    Success = false,
                    ErrorMessage = "Ticker not found"
                });
                return;
            }

            if (request.Side == "buy")
            {
                var userStock = user.UserStocks.FirstOrDefault(us => us.CatalogId == stockCatalog.Id);
                if (userStock == null)
                {
                    userStock = new UserStock
                    {
                        UserId = user.Id,
                        CatalogId = stockCatalog.Id,
                        Quantity = request.Quantity
                    };
                    _context.UserStocks.Add(userStock);
                }
                else
                {
                    userStock.Quantity += request.Quantity;
                }
            }
            else if (request.Side == "sell")
            {
                var userStock = user.UserStocks.FirstOrDefault(us => us.CatalogId == stockCatalog.Id);
                if (userStock == null || userStock.Quantity < request.Quantity)
                {
                    await context.RespondAsync(new OrderResponse
                    {
                        Success = false,
                        ErrorMessage = "Insufficient stock quantity"
                    });
                    return;
                }
                userStock.Quantity -= request.Quantity;
            }
            else
            {
                await context.RespondAsync(new OrderResponse
                {
                    Success = false,
                    ErrorMessage = "Invalid order side"
                });
                return;
            }

            await _context.SaveChangesAsync();

            await context.RespondAsync(new OrderResponse
            {
                Success = true
            });

            _logger.LogInformation($"Order processed successfully for UserId={user.Id}");
        }
    }
}
