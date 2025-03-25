using Microsoft.EntityFrameworkCore;
using OrderService.Entities;

namespace OrderService.DataAccess;

public class LamanDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public LamanDbContext(DbContextOptions<LamanDbContext> options) : base(options)
    {
    }
}
