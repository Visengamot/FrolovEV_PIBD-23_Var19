using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace ShoeStore.WinForms.Data.Migrations
{
    /// <inheritdoc/>
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase().Annotation("Npgsql:PostgresExtension:citext", ",,");
            migrationBuilder.CreateTable(name: "Manufacturers", columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "citext", maxLength: 200, nullable: false),
                Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Manufacturers", x => x.Id);
                table.CheckConstraint(
                    "CK_Manufacturers_Text",
                    "length(btrim(\"Name\")) > 0 AND length(btrim(\"Country\")) > 0"
                );
            });
            migrationBuilder.CreateTable(name: "ShoeSizes", columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Value = table.Column<decimal>(type: "numeric(4,1)", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_ShoeSizes", x => x.Id);
                table.CheckConstraint(
                    "CK_ShoeSizes_Value",
                    "\"Value\" BETWEEN 10 AND 60 AND mod(\"Value\" * 2, 1) = 0"
                );
            });
            migrationBuilder.CreateTable(name: "StockMovements", columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Date = table.Column<DateTime>(type: "date", nullable: false),
                Type = table.Column<int>(type: "integer", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_StockMovements", x => x.Id);
                table.CheckConstraint("CK_StockMovements_Type", "\"Type\" IN (0, 1)");
            });
            migrationBuilder.CreateTable(name: "Shoes", columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Article = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                Category = table.Column<int>(type: "integer", nullable: false),
                Seasons = table.Column<int>(type: "integer", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Shoes", x => x.Id);
                table.CheckConstraint(
                    "CK_Shoes_Enums",
                    "\"Category\" BETWEEN 0 AND 4 AND \"Seasons\" BETWEEN 1 AND 15"
                );
                table.CheckConstraint("CK_Shoes_Price", "\"Price\" > 0");
                table.CheckConstraint(
                    "CK_Shoes_Text",
                    "length(btrim(\"Article\")) > 0 AND length(btrim(\"Name\")) > 0"
                );
                table.ForeignKey(
                    name: "FK_Shoes_Manufacturers_ManufacturerId",
                    column: x => x.ManufacturerId,
                    principalTable: "Manufacturers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict
                );
            });
            migrationBuilder.CreateTable(name: "MovementItems", columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                MovementId = table.Column<Guid>(type: "uuid", nullable: false),
                ShoeId = table.Column<Guid>(type: "uuid", nullable: false),
                ShoeSizeId = table.Column<Guid>(type: "uuid", nullable: false),
                Quantity = table.Column<int>(type: "integer", nullable: false),
                UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_MovementItems", x => x.Id);
                table.CheckConstraint("CK_MovementItems_Positive", "\"Quantity\" > 0 AND \"UnitPrice\" > 0");
                table.ForeignKey(
                    name: "FK_MovementItems_ShoeSizes_ShoeSizeId",
                    column: x => x.ShoeSizeId,
                    principalTable: "ShoeSizes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict
                );
                table.ForeignKey(
                    name: "FK_MovementItems_Shoes_ShoeId",
                    column: x => x.ShoeId,
                    principalTable: "Shoes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict
                );
                table.ForeignKey(
                    name: "FK_MovementItems_StockMovements_MovementId",
                    column: x => x.MovementId,
                    principalTable: "StockMovements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            });
            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_Name",
                table: "Manufacturers",
                column: "Name",
                unique: true
            );
            migrationBuilder.CreateIndex(
                name: "IX_MovementItems_MovementId_ShoeId_ShoeSizeId",
                table: "MovementItems",
                columns: new[]
            {
                "MovementId",
                "ShoeId",
                "ShoeSizeId"
            },
                unique: true
            );
            migrationBuilder.CreateIndex(name: "IX_MovementItems_ShoeId", table: "MovementItems", column: "ShoeId");
            migrationBuilder.CreateIndex(
                name: "IX_MovementItems_ShoeSizeId",
                table: "MovementItems",
                column: "ShoeSizeId"
            );
            migrationBuilder.CreateIndex(name: "IX_Shoes_Article", table: "Shoes", column: "Article", unique: true);
            migrationBuilder.CreateIndex(name: "IX_Shoes_ManufacturerId", table: "Shoes", column: "ManufacturerId");
            migrationBuilder.CreateIndex(name: "IX_ShoeSizes_Value", table: "ShoeSizes", column: "Value", unique: true);
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MovementItems");
            migrationBuilder.DropTable(name: "ShoeSizes");
            migrationBuilder.DropTable(name: "Shoes");
            migrationBuilder.DropTable(name: "StockMovements");
            migrationBuilder.DropTable(name: "Manufacturers");
        }
    }
}
