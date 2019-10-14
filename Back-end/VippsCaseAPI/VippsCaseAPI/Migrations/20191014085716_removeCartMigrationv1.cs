using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VippsCaseAPI.Migrations
{
    public partial class removeCartMigrationv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_carts_CartId",
                table: "Item");

            migrationBuilder.DropTable(
                name: "carts");

            migrationBuilder.DropIndex(
                name: "IX_Item_CartId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Item");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Order",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Item",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "carts",
                columns: table => new
                {
                    CartId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carts", x => x.CartId);
                });

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
    }
}
