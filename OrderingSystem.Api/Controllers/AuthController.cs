using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Application.DTOs.Auth;
using OrderingSystem.Application.DTOs.Users;
using OrderingSystem.Application.Services;

namespace OrderingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IUserService _users;

        public AuthController(IAuthService auth, IUserService users)
        {
            _auth = auth;
            _users = users;
        }

 
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _auth.LoginAsync(dto);
            if (result == null || !result.Success)
                return Unauthorized(result?.Message);

            return Ok(new { token = result.Token });
        }

 
    
        [Authorize]
        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto dto)
        {
            var result = await _users.AddUserAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

   
        [Authorize]
        [HttpPut("users/{id}")]
        public async Task<IActionResult> EditUser(int id, [FromBody] EditUserDto dto)
        {
            var result = await _users.EditUserAsync(id, dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

     
        [Authorize]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _users.DeleteUserAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

         
        [Authorize]
        [HttpPost("users/{userId}/assign-role")]
        public async Task<IActionResult> AssignRole(int userId, [FromBody] AssignRoleDto dto)
        {
            var result = await _users.AssignRoleAsync(userId, dto.RoleId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
