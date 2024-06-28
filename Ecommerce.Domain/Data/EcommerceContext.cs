namespace Ecommerce.Domain.Data
{
    using Ecommerce.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class EcommerceContext : DbContext
    {
        public EcommerceContext() { }

        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
