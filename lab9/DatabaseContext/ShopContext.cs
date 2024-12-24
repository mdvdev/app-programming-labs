namespace DatabaseContext;

using Microsoft.EntityFrameworkCore;
using DatabaseModels;

public class ShopContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Shipment> Shipments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=123456789");
        base.OnConfiguring(optionsBuilder);
    }
}