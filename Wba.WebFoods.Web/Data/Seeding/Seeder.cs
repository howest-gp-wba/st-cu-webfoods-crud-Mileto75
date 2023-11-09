using Microsoft.EntityFrameworkCore;
using Wba.WebFoods.Core.Entities;

namespace Wba.WebFoods.Web.Data.Seeding
{
    public static class Seeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            //put seeding data here
            //work with arrays
            //category array
            var categories = new Category[]
            {
                new Category{Id = 1,Name = "Antipasti" },
                new Category{Id = 2,Name = "Primi" },
                new Category{Id = 3,Name = "Secondi" },
                new Category{Id = 4,Name = "Dessert" },
            };
            //properties array
            var properties = new Property[]
            {
                new Property{Id = 1, Name = "Spicy" },
                new Property{Id = 2, Name = "Vegan" },
                new Property{Id = 3, Name = "Meatlovers" },
                new Property{Id = 4, Name = "Gluten free" },
                new Property{Id = 5, Name = "Sugar free" },
                new Property{Id = 6, Name = "Traditional" },
                new Property{Id = 7, Name = "Modern Italian" },
                new Property{Id = 8, Name = "Fusion" },
            };
            //products array
            var products = new Product[]
            {
                new Product{Id = 1, Name = "Bruschetta mista",Price=10.00M, CategoryId = 1,Description = "Very goodie"},
                new Product{Id = 2, Name = "Affetati rustica",Price=12.50M, CategoryId = 1,Description = "Meat"},
                new Product{Id = 3, Name = "Spaghetti carbonara",Price=13.90M, CategoryId = 2,Description = "A classic"},
                new Product{Id = 4, Name = "Penne all'arrabiata",Price=13.80M, CategoryId = 2,Description = "Hot dish"},
                new Product{Id = 5, Name = "Osso buco alla Milanese",Price=23.80M, CategoryId = 3,Description = "Classic meat"},
                new Product{Id = 6, Name = "Saltimbocca all'Abruzzese",Price=25.80M, CategoryId = 3,Description = "Vegan meat"},
                new Product{Id = 7, Name = "Millefoglie au frutti di bosco",Price=14.80M, CategoryId = 4,Description = "Vegan woodfruit"},
                new Product{Id = 8, Name = "Tartufo all'amaro",Price=12.80M, CategoryId = 4,Description = "Chocolate and alcohol"},
            };
            //productProperties table
            var productProperties = new[]
            {
                new { ProductsId = 1, PropertiesId = 1 },
                new { ProductsId = 1, PropertiesId = 2 },
                new { ProductsId = 2, PropertiesId = 3 },                
                new { ProductsId = 2, PropertiesId = 2 },
                new { ProductsId = 3, PropertiesId = 3 },
                new { ProductsId = 3, PropertiesId = 4 },
                new { ProductsId = 3, PropertiesId = 5 },
            };
            //put seeding data in database
            //use the entity<>().HasData method
            //mind the order/sequence
            modelBuilder.Entity<Category>().HasData(categories);
            modelBuilder.Entity<Property>().HasData(properties);
            modelBuilder.Entity<Product>().HasData(products);
            //the many to many table ProductProperties
            modelBuilder.Entity($"{nameof(Product)}{nameof(Property)}")
                .HasData(productProperties);
        }
    }
}
