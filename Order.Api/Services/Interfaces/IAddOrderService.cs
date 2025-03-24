using Order.Api.Models.DTOs;

namespace Order.Api.Services.Interfaces
{
    public interface IAddOrderService
    {
        Task<(bool Success, string? ErrorMessage, Models.Entities.Order? Order)> CreateOrderAsync(int userId, CreateOrderDto orderDto);
    }
}
