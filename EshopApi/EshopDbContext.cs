using Microsoft.EntityFrameworkCore;

namespace EshopApi;

public class EshopDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public EshopDbContext(DbContextOptions<EshopDbContext> options) :
        base(options)
    {
    }
}
