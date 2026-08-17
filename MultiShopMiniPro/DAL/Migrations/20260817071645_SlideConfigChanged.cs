using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiShopMiniPro.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SlideConfigChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Slides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Slides");
        }
    }
}
