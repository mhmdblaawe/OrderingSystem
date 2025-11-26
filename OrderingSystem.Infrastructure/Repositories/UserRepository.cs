using Microsoft.EntityFrameworkCore;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Domain.Entities;
using OrderingSystem.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly OrderingDbContext _ctx;

    public UserRepository(OrderingDbContext ctx)
    {
        _ctx = ctx;
    }

    public Task<User?> GetByIdAsync(int id)
        => _ctx.Users.FirstOrDefaultAsync(x => x.Id == id);

    public Task<User?> GetByUsernameAsync(string username)
    {
        return _ctx.Users
            .Include(u => u.Roles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task AddAsync(User user)
        => await _ctx.Users.AddAsync(user);

    public Task UpdateAsync(User user)
    {
        _ctx.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(User user)
    {
        _ctx.Users.Remove(user);
        return Task.CompletedTask;
    }

    public Task SaveAsync() => _ctx.SaveChangesAsync();
}
