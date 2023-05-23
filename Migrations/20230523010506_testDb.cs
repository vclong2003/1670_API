using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1670_API.Migrations
{
    /// <inheritdoc />
    public partial class testDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stores_storeID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_userID",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "storeID",
                table: "Orders",
                newName: "StoreID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_userID",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_storeID",
                table: "Orders",
                newName: "IX_Orders_StoreID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stores_StoreID",
                table: "Orders",
                column: "StoreID",
                principalTable: "Stores",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stores_StoreID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "userID");

            migrationBuilder.RenameColumn(
                name: "StoreID",
                table: "Orders",
                newName: "storeID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                newName: "IX_Orders_userID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_StoreID",
                table: "Orders",
                newName: "IX_Orders_storeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stores_storeID",
                table: "Orders",
                column: "storeID",
                principalTable: "Stores",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_userID",
                table: "Orders",
                column: "userID",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
