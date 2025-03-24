using Microsoft.AspNetCore.Mvc;
using Order.Api.Models.DTOs;
using Order.Api.Services.Interfaces;

namespace Order.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly IAddOrderService _addOrderService;

        public OrderController(IAddOrderService addOrderService)
        {
            _addOrderService = addOrderService;
        }

        [HttpPost("add/{userId}")]
        public async Task<IActionResult> Add(int userId, [FromBody] CreateOrderDto orderDto)
        {
            var (success, errorMessage, order) = await _addOrderService.CreateOrderAsync(userId, orderDto);

            if (!success)
            {
                return BadRequest(new { Message = errorMessage });
            }

            return Ok(new
            {
                order.Id,
                order.Ticker,
                order.Quantity,
                order.Total,
                order.CreatedAt
            });
        }

    }
}
