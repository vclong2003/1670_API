using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1670_API.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseVer3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookStore_Books_BookID",
                table: "BookStore");

            migrationBuilder.DropForeignKey(
                name: "FK_BookStore_Store_StoreID",
                table: "BookStore");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Books_BookID",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_User_UserID",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Store_storeID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_userID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Books_BookID",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Order_OrderID",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Store_User_userID",
                table: "Store");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Store",
                table: "Store");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookStore",
                table: "BookStore");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Store",
                newName: "Stores");

            migrationBuilder.RenameTable(
                name: "OrderItem",
                newName: "OrderItems");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "CartItem",
                newName: "CartItems");

            migrationBuilder.RenameTable(
                name: "BookStore",
                newName: "BookStores");

            migrationBuilder.RenameIndex(
                name: "IX_Store_userID",
                table: "Stores",
                newName: "IX_Stores_userID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_BookID",
                table: "OrderItems",
                newName: "IX_OrderItems_BookID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_userID",
                table: "Orders",
                newName: "IX_Orders_userID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_storeID",
                table: "Orders",
                newName: "IX_Orders_storeID");

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_BookID",
                table: "CartItems",
                newName: "IX_CartItems_BookID");

            migrationBuilder.RenameIndex(
                name: "IX_BookStore_BookID",
                table: "BookStores",
                newName: "IX_BookStores_BookID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stores",
                table: "Stores",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                columns: new[] { "OrderID", "BookID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems",
                columns: new[] { "UserID", "BookID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookStores",
                table: "BookStores",
                columns: new[] { "StoreID", "BookID" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookStores_Books_BookID",
                table: "BookStores",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookStores_Stores_StoreID",
                table: "BookStores",
                column: "StoreID",
                principalTable: "Stores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Books_BookID",
                table: "CartItems",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Users_UserID",
                table: "CartItems",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Books_BookID",
                table: "OrderItems",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderID",
                table: "OrderItems",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Users_userID",
                table: "Stores",
                column: "userID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookStores_Books_BookID",
                table: "BookStores");

            migrationBuilder.DropForeignKey(
                name: "FK_BookStores_Stores_StoreID",
                table: "BookStores");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Books_BookID",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Users_UserID",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Books_BookID",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderID",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stores_storeID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_userID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Users_userID",
                table: "Stores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stores",
                table: "Stores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookStores",
                table: "BookStores");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Stores",
                newName: "Store");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "OrderItem");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "CartItem");

            migrationBuilder.RenameTable(
                name: "BookStores",
                newName: "BookStore");

            migrationBuilder.RenameIndex(
                name: "IX_Stores_userID",
                table: "Store",
                newName: "IX_Store_userID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_userID",
                table: "Order",
                newName: "IX_Order_userID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_storeID",
                table: "Order",
                newName: "IX_Order_storeID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_BookID",
                table: "OrderItem",
                newName: "IX_OrderItem_BookID");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_BookID",
                table: "CartItem",
                newName: "IX_CartItem_BookID");

            migrationBuilder.RenameIndex(
                name: "IX_BookStores_BookID",
                table: "BookStore",
                newName: "IX_BookStore_BookID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Store",
                table: "Store",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem",
                columns: new[] { "OrderID", "BookID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem",
                columns: new[] { "UserID", "BookID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookStore",
                table: "BookStore",
                columns: new[] { "StoreID", "BookID" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookStore_Books_BookID",
                table: "BookStore",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookStore_Store_StoreID",
                table: "BookStore",
                column: "StoreID",
                principalTable: "Store",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Books_BookID",
                table: "CartItem",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_User_UserID",
                table: "CartItem",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Store_storeID",
                table: "Order",
                column: "storeID",
                principalTable: "Store",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_userID",
                table: "Order",
                column: "userID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Books_BookID",
                table: "OrderItem",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Order_OrderID",
                table: "OrderItem",
                column: "OrderID",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Store_User_userID",
                table: "Store",
                column: "userID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
