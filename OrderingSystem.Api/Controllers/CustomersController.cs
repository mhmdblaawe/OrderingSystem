using Microsoft.AspNetCore.Mvc;

namespace OrderingSystem.Api.Controllers
{
    using global::OrderingSystem.Application.DTOs.Customers;
    using global::OrderingSystem.Application.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
  

    namespace OrderingSystem.Api.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize] 
        public class CustomersController : ControllerBase
        {
            private readonly ICustomerService _service;

            public CustomersController(ICustomerService service)
            {
                _service = service;
            }

            [HttpGet("GetCustomersPaged")]
            public async Task<IActionResult> GetPaged(
                int pageNumber = 1,
                int pageSize = 10,
                string? name = null,
                string? email = null)
            {
                var data = await _service.GetPagedAsync(pageNumber, pageSize, name, email);
          
                return Ok(new { total = data.TotalCount, items = data.Items });
            }


            [HttpGet("GetCustomerById/{id}")]
            public async Task<IActionResult> GetCustomerById(int id)
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null) return NotFound();
                return Ok(result);
            }


            [Authorize(Roles = "Admin")]
            [HttpPost("CreateCustomer")]
            public async Task<IActionResult> CreateCustomer(CreateCustomerDto dto)
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetCustomerById), new { id = created.Id }, created);
            }
            [Authorize(Roles = "Admin")]
            
            [HttpDelete("DeleteCustomer/{id}")]
            public async Task<IActionResult> DeleteCustomer(int id)
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
        }
    }

}
