using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;


namespace ProductApi.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja dla serializacji/deserializacji JSON
            modelBuilder.Entity<Product>()
                .Property(p => p.Category)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }), // Zapis jako JSON
                    v => JsonSerializer.Deserialize<string[]>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) // Odczyt jako tablica
                );
        }
    
    public DbSet<User> Users { get; set; } // Nowa tabela

      
    }
}
