using LibraryManagement.Business.DTOs.Order;
using LibraryManagement.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var orderId = await _orderService.CreateOrderAsync(dto);
            return Ok(new { Message = "Order created successfully", OrderId = orderId });
        }

        [HttpPost("test-rollback")]
        public async Task<IActionResult> TestRollBack([FromBody] CreateOrderDto dto)
        {
            var orderId = await _orderService.CreateOrderWithFailureAsync(dto);
            return Ok(orderId);
        }
    }
}
