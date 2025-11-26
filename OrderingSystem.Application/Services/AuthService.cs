using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderingSystem.Application.DTOs.Auth;
 using OrderingSystem.Application.Interfaces;
using OrderingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository users, IConfiguration config)
        {
            _users = users;
            _config = config;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _users.GetByUsernameAsync(dto.Username);

            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password."
                };
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Login successful.",
                Token = token,
                Username = user.Username,
                Roles = user.Roles.Select(r => r.Role!.Name).ToList()
            };
        }


        //public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        //{
        //    var existing = await _users.GetByUsernameAsync(dto.Username);
        //    if (existing != null)
        //    {
        //        return new AuthResponseDto
        //        {
        //            Success = false,
        //            Message = "Username already exists."
        //        };
        //    }

        //    var hash = HashPassword(dto.Password);
        //    var user = User.Create(dto.Username, hash);
        //    await _users.AddAsync(user);
        //    await _users.SaveAsync();
        //    foreach (var roleName in dto.Roles)
        //    {
        //        var role = await _users.GetRoleAsync(roleName);
        //        if (role == null)
        //        {
        //            return new AuthResponseDto
        //            {
        //                Success = false,
        //                Message = $"Role '{roleName}' does not exist."
        //            };
        //        }

        //        await _users.AddRoleToUserAsync(user.Id, role.Id);
        //    }

        //    await _users.SaveAsync();

        //    var token = GenerateJwtToken(user);

        //    return new AuthResponseDto
        //    {
        //        Success = true,
        //        Message = "Registration successful.",
        //        Token = token,
        //        Username = user.Username,
        //        Roles = dto.Roles
        //    };
        //}


        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("uid", user.Id.ToString())
            };

            foreach (var r in user.Roles)
                claims.Add(new Claim(ClaimTypes.Role, r.Role!.Name));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}
