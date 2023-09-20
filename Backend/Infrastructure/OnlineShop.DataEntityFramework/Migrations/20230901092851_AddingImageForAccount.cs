using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShopBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddingImageForAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Accounts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Accounts");
        }
    }
}
