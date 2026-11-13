using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShoeStore.WinForms.Entities;

namespace ShoeStore.WinForms.Data;

public sealed class DataContext : DbContext
{
    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();
    public DbSet<Shoe> Shoes => Set<Shoe>();
    public DbSet<ShoeSize> ShoeSizes => Set<ShoeSize>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<MovementItem> MovementItems => Set<MovementItem>();

    public DataContext() { }
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public static IConfiguration LoadConfiguration() => new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .AddJsonFile("appsettings.Local.json", optional: true)
        .Build();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = Environment.GetEnvironmentVariable("SHOESTORE_CONNECTION_STRING")
                ?? LoadConfiguration().GetConnectionString("ShoeStore");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("citext");
        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.Name).HasColumnType("citext");
            entity.HasIndex(x => x.Name).IsUnique();
            entity.ToTable(table => table.HasCheckConstraint("CK_Manufacturers_Text", "length(btrim(\"Name\")) > 0 AND length(btrim(\"Country\")) > 0"));
        });
        modelBuilder.Entity<Shoe>(entity =>
        {
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.Article).HasColumnType("citext");
            entity.HasIndex(x => x.Article).IsUnique();
            entity.HasOne(x => x.Manufacturer).WithMany().HasForeignKey(x => x.ManufacturerId).OnDelete(DeleteBehavior.Restrict);
            entity.ToTable(table =>
            {
                table.HasCheckConstraint("CK_Shoes_Price", "\"Price\" > 0");
                table.HasCheckConstraint("CK_Shoes_Enums", "\"Category\" BETWEEN 0 AND 4 AND \"Seasons\" BETWEEN 1 AND 15");
                table.HasCheckConstraint("CK_Shoes_Text", "length(btrim(\"Article\")) > 0 AND length(btrim(\"Name\")) > 0");
            });
        });
        modelBuilder.Entity<ShoeSize>(entity =>
        {
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.HasIndex(x => x.Value).IsUnique();
            entity.ToTable(table => table.HasCheckConstraint("CK_ShoeSizes_Value", "\"Value\" BETWEEN 10 AND 60 AND mod(\"Value\" * 2, 1) = 0"));
        });
        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.HasMany(x => x.Items).WithOne(x => x.Movement).HasForeignKey(x => x.MovementId).OnDelete(DeleteBehavior.Cascade);
            entity.Navigation(x => x.Items).HasField("_items").UsePropertyAccessMode(PropertyAccessMode.Field);
            entity.ToTable(table => table.HasCheckConstraint("CK_StockMovements_Type", "\"Type\" IN (0, 1)"));
        });
        modelBuilder.Entity<MovementItem>(entity =>
        {
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.HasOne(x => x.Shoe).WithMany().HasForeignKey(x => x.ShoeId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.ShoeSize).WithMany().HasForeignKey(x => x.ShoeSizeId).OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(x => new { x.MovementId, x.ShoeId, x.ShoeSizeId }).IsUnique();
            entity.ToTable(table => table.HasCheckConstraint("CK_MovementItems_Positive", "\"Quantity\" > 0 AND \"UnitPrice\" > 0"));
        });
    }
}
