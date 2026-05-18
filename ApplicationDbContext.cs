using Microsoft.EntityFrameworkCore;
using GestionStock.Models;

namespace GestionStock.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options
        ) : base(options)
        {
        }

        // Tables

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SaleDetail> SaleDetails { get; set; }

        // Configuration avancée

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product → Category

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // SaleDetail → Product

            modelBuilder.Entity<SaleDetail>()
                .HasOne(sd => sd.Product)
                .WithMany()
                .HasForeignKey(sd => sd.ProductId);

            // SaleDetail → Sale

            modelBuilder.Entity<SaleDetail>()
                .HasOne(sd => sd.Sale)
                .WithMany(s => s.SaleDetails)
                .HasForeignKey(sd => sd.SaleId);
        }
    }
}