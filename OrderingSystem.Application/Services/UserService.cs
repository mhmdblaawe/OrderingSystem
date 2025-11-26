using OrderingSystem.Application.DTOs.Users;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Application.Shared;
using OrderingSystem.Domain.Entities;

namespace OrderingSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IRoleRepository _roles;

        public UserService(IUserRepository users, IRoleRepository roles)
        {
            _repo = users;
            _roles = roles;
        }

        public async Task<Result> AddUserAsync(AddUserDto dto)
        {
            var existing = await _repo.GetByUsernameAsync(dto.Username);
            if (existing != null)
                return new Result { Success = false, Message = "Username already exists" };

            var user = User.Create(dto.Username, dto.Password);

            await _repo.AddAsync(user);
            await _repo.SaveAsync();


            return new Result { Success = true, Message = "User created successfully" };
        }


        public async Task<Result> EditUserAsync(int id, EditUserDto dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                return new Result { Success = false, Message = "User not found" };

            user.UpdateUsername(dto.Username);

            await _repo.SaveAsync();

            return new Result { Success = true, Message = "User updated" };
        }

        public async Task<Result> DeleteUserAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                return new Result { Success = false, Message = "User not found" };

            user.Deactivate();

            await _repo.SaveAsync();

            return new Result { Success = true, Message = "User deleted" };
        }


        public async Task<Result> AssignRoleAsync(int userId, int roleId)
        {
            var user = await _repo.GetByIdAsync(userId);
            if (user == null)
                return new Result { Success = false, Message = "User not found" };

            var role = await _roles.GetRoleByIdAsync(roleId);
            if (role == null)
                return new Result { Success = false, Message = "Role not found" };

            await _roles.AssignRoleToUserAsync(userId, roleId);
            await _roles.SaveAsync();

            return new Result { Success = true, Message = "Role assigned" };
        }
    }
}
