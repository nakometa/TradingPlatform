using MassTransit;
using Order.Api.Data;
using Order.Api.Models.DTOs;
using Order.Api.Services.Interfaces;
using Shared.Contracts;

namespace Order.Api.Services
{
    public class AddOrderService : IAddOrderService
    {
        private readonly OrderDbContext _context;
        private readonly IPriceCache _priceCache;
        private readonly IRequestClient<OrderRequest> _requestClient;
        private readonly ILogger<AddOrderService> _logger;

        public AddOrderService(OrderDbContext context, 
            IPriceCache priceCache, 
            IRequestClient<OrderRequest> requestClient, 
            ILogger<AddOrderService> logger)
        {
            _context = context;
            _priceCache = priceCache;
            _requestClient = requestClient;
            _logger = logger;
        }

        async Task<(bool Success, string? ErrorMessage, Models.Entities.Order? Order)> IAddOrderService.CreateOrderAsync(int userId, CreateOrderDto orderDto)
        {
            if (!_priceCache.TryGetPrice(orderDto.Ticker, out decimal pricePerUnit))
            {
                return (false, $"No price available for ticker {orderDto.Ticker}", null);
            }

            var orderRequest = new OrderRequest
            {
                UserId = userId,
                Ticker = orderDto.Ticker,
                Quantity = orderDto.Quantity,
                Side = orderDto.Side
            };

            var response = await _requestClient.GetResponse<OrderResponse>(orderRequest);

            if (!response.Message.Success)
            {
                _logger.LogWarning("Order validation failed: {Message}", response.Message.ErrorMessage);

                return (false, response.Message.ErrorMessage, null);
            }

            var totalPrice = pricePerUnit * orderDto.Quantity;

            var order = new Models.Entities.Order
            {
                UserId = userId,
                Ticker = orderDto.Ticker,
                Quantity = orderDto.Quantity,
                Side = orderDto.Side,
                PricePerUnit = pricePerUnit,
                Total = totalPrice,
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order created: {order.Id} - {order.Ticker} - {order.Quantity} at {order.PricePerUnit}");

            return (true, null, order);
        }
    }
}
