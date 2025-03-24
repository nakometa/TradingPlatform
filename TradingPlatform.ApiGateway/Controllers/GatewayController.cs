using Microsoft.AspNetCore.Mvc;

namespace TradingPlatform.ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GatewayController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("order/add/{userId}")]
        public async Task<IActionResult> AddOrder(int userId, [FromBody] object orderDto)
        {
            var client = _httpClientFactory.CreateClient("OrderService");
            var response = await client.PostAsJsonAsync($"/api/order/add/{userId}", orderDto);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

        [HttpGet("portfolio/{userId}")]
        public async Task<IActionResult> GetPortfolio(int userId)
        {
            var client = _httpClientFactory.CreateClient("PortfolioService");
            var response = await client.GetAsync($"/api/portfolio/{userId}");
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }
    }
}
