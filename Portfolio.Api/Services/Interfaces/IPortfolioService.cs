using Portfolio.Api.Models.DTOs;

namespace Portfolio.Api.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<(decimal TotalValue, List<UserStockDto> Stocks)> GetUserPortfolioAsync(int userId);
    }
}
