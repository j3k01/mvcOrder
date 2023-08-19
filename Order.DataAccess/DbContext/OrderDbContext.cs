using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Order.Model.Models;
using Order.Model.Enums;

namespace Order.DataAccess.DbContext
{
    public class OrderDbContext : IdentityDbContext<IdentityUser>
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Symbol = "Symbol",
                    Name = "Apple",
                    ShortDescription = "Red fruit",
                    LongDescription = "Awesome, delicious, almost perfect fruit",
                    NetPrice = 2.0,
                    GrossPrice = 3.0,
                    ActiveState = true,
                    VATRate = VATRate.LowVAT,
                    MeasureUnit = "Kilograms",
                    MainImageId = 1,
                },
                new Product
                {
                    Id = 2,
                    Symbol = "Symbol",
                    Name = "Banana",
                    ShortDescription = "Yellow fruit",
                    LongDescription = "Yellow fruit, monkeys love them",
                    NetPrice = 3.0,
                    GrossPrice = 4.0,
                    ActiveState = true,
                    VATRate = VATRate.MediumVAT,
                    MeasureUnit = "Kilograms",
                    MainImageId = 1,
                }
                );
        }
    }
}

