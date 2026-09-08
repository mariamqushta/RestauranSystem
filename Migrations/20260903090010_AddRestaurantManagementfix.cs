using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace restaurantAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantManagementfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Restaurants_RestaurantId1",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_RestaurantId1",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "RestaurantId1",
                table: "InventoryItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "RestaurantTables",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantId",
                table: "InventoryItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_RestaurantId",
                table: "InventoryItems",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Restaurants_RestaurantId",
                table: "InventoryItems",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Restaurants_RestaurantId",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_RestaurantId",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "RestaurantTables");

            migrationBuilder.AlterColumn<long>(
                name: "RestaurantId",
                table: "InventoryItems",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId1",
                table: "InventoryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_RestaurantId1",
                table: "InventoryItems",
                column: "RestaurantId1");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Restaurants_RestaurantId1",
                table: "InventoryItems",
                column: "RestaurantId1",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_AspNetUsers_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
