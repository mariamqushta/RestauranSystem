using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace restaurantAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixInventoryItemId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove the primary key first
            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems");

            // Change ID from bigint to int
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "InventoryItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            // Add the primary key back
            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems");

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "InventoryItems",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems",
                column: "ID");
        }
    }
}
