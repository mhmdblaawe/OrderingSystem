using OrderingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        //Task<Role?> GetRoleAsync(string roleName);
        //Task AddRoleToUserAsync(int userId, int roleId);
        Task<User?> GetByIdAsync(int id);
        Task RemoveAsync(User user);

        Task SaveAsync();
    }

}
