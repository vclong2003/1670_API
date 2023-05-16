using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1670_API.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseVer1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_userId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "cusID",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Order",
                newName: "userID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_userId",
                table: "Order",
                newName: "IX_Order_userID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_userID",
                table: "Order",
                column: "userID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_userID",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "Order",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_userID",
                table: "Order",
                newName: "IX_Order_userId");

            migrationBuilder.AddColumn<int>(
                name: "cusID",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_userId",
                table: "Order",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
