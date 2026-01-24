using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeerStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShopModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shop",
                columns: table => new
                {
                    IdShop = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ShopType = table.Column<int>(type: "int", nullable: false),
                    ShopStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shop", x => x.IdShop);
                });

            migrationBuilder.CreateTable(
                name: "ShopAddress",
                columns: table => new
                {
                    IdShopAddress = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(69)", maxLength: 69, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(69)", maxLength: 69, nullable: false),
                    District = table.Column<string>(type: "nvarchar(69)", maxLength: 69, nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(69)", maxLength: 69, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopAddress", x => x.IdShopAddress);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shop_OwnerId",
                table: "Shop",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Shop_Slug",
                table: "Shop",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopAddress_ShopId",
                table: "ShopAddress",
                column: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shop");

            migrationBuilder.DropTable(
                name: "ShopAddress");
        }
    }
}
