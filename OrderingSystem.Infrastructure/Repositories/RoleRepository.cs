using Microsoft.EntityFrameworkCore;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Domain.Entities;
using OrderingSystem.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly OrderingDbContext _ctx;

        public RoleRepository(OrderingDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<Role?> GetRoleByIdAsync(int roleId)
            => _ctx.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

        public async Task AssignRoleToUserAsync(int userId, int roleId)
        {
            var ur = new UserRole(userId, roleId);
            await _ctx.UserRoles.AddAsync(ur);
        }

        public Task SaveAsync()
            => _ctx.SaveChangesAsync();
    }

}
