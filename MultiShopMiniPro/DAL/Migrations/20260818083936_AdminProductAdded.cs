using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiShopMiniPro.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AdminProductAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Products");
        }
    }
}
