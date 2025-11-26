using OrderingSystem.Application.DTOs.Users;
using OrderingSystem.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Services
{
    public interface IUserService
    {
        Task<Result> AddUserAsync(AddUserDto dto);
        Task<Result> EditUserAsync(int id, EditUserDto dto);
        Task<Result> DeleteUserAsync(int id);
        Task<Result> AssignRoleAsync(int userId, int roleId);
    }
}
