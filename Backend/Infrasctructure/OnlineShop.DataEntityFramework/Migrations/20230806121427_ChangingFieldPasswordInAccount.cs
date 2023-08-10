using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShopBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangingFieldPasswordInAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Accounts",
                newName: "HashedPassword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashedPassword",
                table: "Accounts",
                newName: "Password");
        }
    }
}
