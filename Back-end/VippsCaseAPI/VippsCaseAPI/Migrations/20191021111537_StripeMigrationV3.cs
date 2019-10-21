using Microsoft.EntityFrameworkCore.Migrations;

namespace VippsCaseAPI.Migrations
{
    public partial class StripeMigrationV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdempotencyToken",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdempotencyToken",
                table: "Order");
        }
    }
}
