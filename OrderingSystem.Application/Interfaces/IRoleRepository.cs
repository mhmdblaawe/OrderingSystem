using OrderingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task AssignRoleToUserAsync(int userId, int roleId);
        Task SaveAsync();
    }
}
