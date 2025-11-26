using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Application.DTOs.Orders;
using OrderingSystem.Application.Services;

namespace OrderingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet("GetOrdersPaged")]
        public async Task<IActionResult> GetPaged(
            int pageNumber = 1,
            int pageSize = 10,
            int? customerId = null,
            int? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var items = await _service.GetPagedAsync(pageNumber, pageSize, customerId, status, startDate, endDate);

            return Ok(new { items.TotalCount, items.Items });
        }

        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetFullAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            dto.OrderId = id;  

            var result = await _service.UpdateStatusAsync(dto);

            return Ok(new { Success = true });
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
    }
}
