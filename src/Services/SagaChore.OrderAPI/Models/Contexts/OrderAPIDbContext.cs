using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SagaChore.OrderAPI.Models.Contexts;

public class OrderAPIDbContext : DbContext
{
    public OrderAPIDbContext(DbContextOptions<OrderAPIDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
