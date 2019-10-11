using Microsoft.EntityFrameworkCore.Migrations;

namespace VippsCaseAPI.Migrations
{
    public partial class ItemsInCartMigrationv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Item",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_CartId",
                table: "Item",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_carts_CartId",
                table: "Item",
                column: "CartId",
                principalTable: "carts",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_carts_CartId",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_CartId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Item");
        }
    }
}
