using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1670_API.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseVer5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Users_userID",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_userID",
                table: "Stores");

            migrationBuilder.AlterColumn<int>(
                name: "userID",
                table: "Stores",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_userID",
                table: "Stores",
                column: "userID",
                unique: true,
                filter: "[userID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Users_userID",
                table: "Stores",
                column: "userID",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Users_userID",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_userID",
                table: "Stores");

            migrationBuilder.AlterColumn<int>(
                name: "userID",
                table: "Stores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_userID",
                table: "Stores",
                column: "userID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Users_userID",
                table: "Stores",
                column: "userID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
