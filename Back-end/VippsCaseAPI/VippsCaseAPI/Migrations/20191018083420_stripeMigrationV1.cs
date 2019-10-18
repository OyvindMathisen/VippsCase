using Microsoft.EntityFrameworkCore.Migrations;

namespace VippsCaseAPI.Migrations
{
    public partial class stripeMigrationV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "PhoneNr",
                table: "User",
                newName: "AddressLineTwo");

            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "User",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "User",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "User",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "AddressLineOne",
                table: "User",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "User",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "User",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StripeChargeToken",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLineOne",
                table: "User");

            migrationBuilder.DropColumn(
                name: "City",
                table: "User");

            migrationBuilder.DropColumn(
                name: "County",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StripeChargeToken",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "User",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "User",
                newName: "Firstname");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "User",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "AddressLineTwo",
                table: "User",
                newName: "PhoneNr");

            migrationBuilder.AddColumn<int>(
                name: "Total",
                table: "Order",
                nullable: false,
                defaultValue: 0);
        }
    }
}
