using Microsoft.EntityFrameworkCore;
using OrderingSystem.Domain.DbModels;
using OrderingSystem.Domain.Entities;

namespace OrderingSystem.Infrastructure.Persistence;

public class OrderingDbContext : DbContext
{
    public OrderingDbContext(DbContextOptions<OrderingDbContext> options)
        : base(options) { }

    public DbSet<Customers> Customers => Set<Customers>();
    public DbSet<Products> Products => Set<Products>();
    public DbSet<Orders> Orders => Set<Orders>();
    public DbSet<OrderItems> OrderItems => Set<OrderItems>();
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

protected override void OnModelCreating(ModelBuilder b)
{
    b.Entity<UserRole>()
     .HasKey(ur => new { ur.UserId, ur.RoleId });

    b.Entity<UserRole>()
     .HasOne(ur => ur.User)
     .WithMany(u => u.Roles)
     .HasForeignKey(ur => ur.UserId);

    b.Entity<UserRole>()
     .HasOne(ur => ur.Role)
     .WithMany()
     .HasForeignKey(ur => ur.RoleId);
}
    }

 
