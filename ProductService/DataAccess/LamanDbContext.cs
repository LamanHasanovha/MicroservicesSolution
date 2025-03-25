using Microsoft.EntityFrameworkCore;
using ProductService.Entities;

namespace ProductService.DataAccess;

public class LamanDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public LamanDbContext(DbContextOptions<LamanDbContext> options) : base(options)
    {
    }
}
