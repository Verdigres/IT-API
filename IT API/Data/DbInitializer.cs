using ProductApi.Models;
using System.Linq;

namespace ProductApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ProductContext context)
        {
            // Upewnij się, że baza danych jest utworzona
            context.Database.EnsureCreated();

            // Sprawdź, czy istnieją już produkty
            if (context.Products.Any())
            {
                return; // Dane zostały już zainicjalizowane
            }

            var products = new Product[]
            {
                new Product { Name = "Product 1", Price = 10.99M },
                new Product { Name = "Product 2", Price = 20.99M },
                new Product { Name = "Product 3", Price = 30.99M },
                new Product { Name = "Product 4", Price = 40.99M }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
