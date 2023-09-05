using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShopBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFieldQuantityInCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Guantity",
                table: "CartItems",
                newName: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "CartItems",
                newName: "Guantity");
        }
    }
}
