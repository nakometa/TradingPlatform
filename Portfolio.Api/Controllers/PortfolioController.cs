using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Services.Interfaces;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPortfolio(int userId)
        {
            var (totalValue, stocks) = await _portfolioService.GetUserPortfolioAsync(userId);
            return Ok(new { TotalValue = totalValue, Stocks = stocks });
        }
    }

}
