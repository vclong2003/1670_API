using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1670_API.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseVer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Store_storeID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_storeID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "storeID",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "ownerID",
                table: "Store",
                newName: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Store_userID",
                table: "Store",
                column: "userID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Store_User_userID",
                table: "Store",
                column: "userID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_User_userID",
                table: "Store");

            migrationBuilder.DropIndex(
                name: "IX_Store_userID",
                table: "Store");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "Store",
                newName: "ownerID");

            migrationBuilder.AddColumn<int>(
                name: "storeID",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_storeID",
                table: "User",
                column: "storeID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Store_storeID",
                table: "User",
                column: "storeID",
                principalTable: "Store",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
