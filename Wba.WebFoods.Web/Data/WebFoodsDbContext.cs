using Microsoft.EntityFrameworkCore;
using Wba.WebFoods.Core.Entities;
using Wba.WebFoods.Web.Data.Seeding;

namespace Wba.WebFoods.Web.Data
{
    public class WebFoodsDbContext : DbContext
    {
        //define DbSets that represent tables
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<User> Users { get; set; }
        public WebFoodsDbContext(DbContextOptions<WebFoodsDbContext> 
            options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //fluent api configuration
            //configure products
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);
            //fix decimal price property
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("money");
            //custom primary key for many to many
            modelBuilder.Entity<OrderLine>()
                .HasKey(o => new { o.UserId, o.ProductId });
            //call the seeder
            Seeder.Seed(modelBuilder);
        }
    }
}
