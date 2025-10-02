using Microsoft.EntityFrameworkCore;
using Team_06.Data.Entities;

namespace Team_06.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for entities
        public DbSet<Product> Products { get; set; }
        public DbSet<Tool> Tools { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships and constraints
            ConfigureProduct(modelBuilder);
            ConfigureTool(modelBuilder);
        }

        private static void ConfigureProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductID);
                entity.Property(e => e.rowguid).HasDefaultValueSql("(newid())");
                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");
                entity.HasIndex(e => e.rowguid).IsUnique();
                entity.HasIndex(e => e.ProductNumber).IsUnique();
                entity.HasIndex(e => e.Name);
            });
        }

        private static void ConfigureTool(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tool>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(36);
                entity.Property(e => e.FriendlyName).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Make).HasMaxLength(100);
                entity.Property(e => e.Model).HasMaxLength(100);
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.Supplier).HasMaxLength(100);
                entity.Property(e => e.Currency).HasMaxLength(3);
                entity.Property(e => e.UnitCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.QRData).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
                
                // Indexes for better query performance
                entity.HasIndex(e => e.FriendlyName);
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.Make);
                entity.HasIndex(e => e.Model);
            });
        }
    }
}
