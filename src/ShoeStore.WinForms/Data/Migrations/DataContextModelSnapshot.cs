
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ShoeStore.WinForms.Data;

#nullable disable
namespace ShoeStore.WinForms.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11").HasAnnotation("Relational:MaxIdentifierLength", 63);
            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "citext");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);
            modelBuilder.Entity("ShoeStore.WinForms.Entities.Manufacturer", b =>
            {
                b.Property<Guid>("Id").HasColumnType("uuid");
                b.Property<string>("Country").IsRequired().HasMaxLength(100).HasColumnType("character varying(100)");
                b.Property<string>("Name").IsRequired().HasMaxLength(200).HasColumnType("citext");
                b.HasKey("Id");
                b.HasIndex("Name").IsUnique();
                b.ToTable("Manufacturers", t =>
                {
                    t.HasCheckConstraint(
                        "CK_Manufacturers_Text",
                        "length(btrim(\"Name\")) > 0 AND length(btrim(\"Country\")) > 0"
                    );
                });
            });
            modelBuilder.Entity("ShoeStore.WinForms.Entities.MovementItem", b =>
            {
                b.Property<Guid>("Id").HasColumnType("uuid");
                b.Property<Guid>("MovementId").HasColumnType("uuid");
                b.Property<int>("Quantity").HasColumnType("integer");
                b.Property<Guid>("ShoeId").HasColumnType("uuid");
                b.Property<Guid>("ShoeSizeId").HasColumnType("uuid");
                b.Property<decimal>("UnitPrice").HasColumnType("numeric(18,2)");
                b.HasKey("Id");
                b.HasIndex("ShoeId");
                b.HasIndex("ShoeSizeId");
                b.HasIndex("MovementId", "ShoeId", "ShoeSizeId").IsUnique();
                b.ToTable("MovementItems", t =>
                {
                    t.HasCheckConstraint("CK_MovementItems_Positive", "\"Quantity\" > 0 AND \"UnitPrice\" > 0");
                });
            });
            modelBuilder.Entity("ShoeStore.WinForms.Entities.Shoe", b =>
            {
                b.Property<Guid>("Id").HasColumnType("uuid");
                b.Property<string>("Article").IsRequired().HasMaxLength(100).HasColumnType("citext");
                b.Property<int>("Category").HasColumnType("integer");
                b.Property<Guid>("ManufacturerId").HasColumnType("uuid");
                b.Property<string>("Name").IsRequired().HasMaxLength(200).HasColumnType("character varying(200)");
                b.Property<decimal>("Price").HasColumnType("numeric(18,2)");
                b.Property<int>("Seasons").HasColumnType("integer");
                b.HasKey("Id");
                b.HasIndex("Article").IsUnique();
                b.HasIndex("ManufacturerId");
                b.ToTable("Shoes", t =>
                {
                    t.HasCheckConstraint(
                        "CK_Shoes_Enums",
                        "\"Category\" BETWEEN 0 AND 4 AND \"Seasons\" BETWEEN 1 AND 15"
                    );
                    t.HasCheckConstraint("CK_Shoes_Price", "\"Price\" > 0");
                    t.HasCheckConstraint(
                        "CK_Shoes_Text",
                        "length(btrim(\"Article\")) > 0 AND length(btrim(\"Name\")) > 0"
                    );
                });
            });
            modelBuilder.Entity("ShoeStore.WinForms.Entities.ShoeSize", b =>
            {
                b.Property<Guid>("Id").HasColumnType("uuid");
                b.Property<decimal>("Value").HasColumnType("numeric(4,1)");
                b.HasKey("Id");
                b.HasIndex("Value").IsUnique();
                b.ToTable("ShoeSizes", t =>
                {
                    t.HasCheckConstraint(
                        "CK_ShoeSizes_Value",
                        "\"Value\" BETWEEN 10 AND 60 AND mod(\"Value\" * 2, 1) = 0"
                    );
                });
            });
            modelBuilder.Entity("ShoeStore.WinForms.Entities.StockMovement", b =>
            {
                b.Property<Guid>("Id").HasColumnType("uuid");
                b.Property<DateTime>("Date").HasColumnType("date");
                b.Property<int>("Type").HasColumnType("integer");
                b.HasKey("Id");
                b.ToTable("StockMovements", t =>
                {
                    t.HasCheckConstraint("CK_StockMovements_Type", "\"Type\" IN (0, 1)");
                });
            });
            modelBuilder.Entity("ShoeStore.WinForms.Entities.MovementItem", b =>
            {
                b.HasOne("ShoeStore.WinForms.Entities.StockMovement", "Movement")
                    .WithMany("Items")
                    .HasForeignKey("MovementId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
                b.HasOne("ShoeStore.WinForms.Entities.Shoe", "Shoe")
                    .WithMany()
                    .HasForeignKey("ShoeId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
                b.HasOne("ShoeStore.WinForms.Entities.ShoeSize", "ShoeSize")
                    .WithMany()
                    .HasForeignKey("ShoeSizeId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
                b.Navigation("Movement");
                b.Navigation("Shoe");
                b.Navigation("ShoeSize");
            });
            modelBuilder.Entity("ShoeStore.WinForms.Entities.Shoe", b =>
            {
                b.HasOne("ShoeStore.WinForms.Entities.Manufacturer", "Manufacturer")
                    .WithMany()
                    .HasForeignKey("ManufacturerId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
                b.Navigation("Manufacturer");
            });
            modelBuilder.Entity("ShoeStore.WinForms.Entities.StockMovement", b =>
            {
                b.Navigation("Items");
            });
#pragma warning restore 612, 618
        }
    }
}
