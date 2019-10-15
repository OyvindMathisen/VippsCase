using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VippsCaseAPI.Migrations
{
    public partial class InitialMigrationv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_passwords_users_UserId",
                table: "passwords");

            migrationBuilder.DropForeignKey(
                name: "FK_paymentInfos_users_UserId",
                table: "paymentInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_paymentInfos",
                table: "paymentInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_passwords",
                table: "passwords");

            migrationBuilder.RenameTable(
                name: "paymentInfos",
                newName: "PaymentInfo");

            migrationBuilder.RenameTable(
                name: "passwords",
                newName: "Password");

            migrationBuilder.RenameIndex(
                name: "IX_paymentInfos_UserId",
                table: "PaymentInfo",
                newName: "IX_PaymentInfo_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_passwords_UserId",
                table: "Password",
                newName: "IX_Password_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentInfo",
                table: "PaymentInfo",
                column: "PaymentInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Password",
                table: "Password",
                column: "PasswordId");

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Total = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Quantity = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItem_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ItemId",
                table: "OrderItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Password_users_UserId",
                table: "Password",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInfo_users_UserId",
                table: "PaymentInfo",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Password_users_UserId",
                table: "Password");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInfo_users_UserId",
                table: "PaymentInfo");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentInfo",
                table: "PaymentInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Password",
                table: "Password");

            migrationBuilder.RenameTable(
                name: "PaymentInfo",
                newName: "paymentInfos");

            migrationBuilder.RenameTable(
                name: "Password",
                newName: "passwords");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentInfo_UserId",
                table: "paymentInfos",
                newName: "IX_paymentInfos_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Password_UserId",
                table: "passwords",
                newName: "IX_passwords_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_paymentInfos",
                table: "paymentInfos",
                column: "PaymentInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_passwords",
                table: "passwords",
                column: "PasswordId");

            migrationBuilder.AddForeignKey(
                name: "FK_passwords_users_UserId",
                table: "passwords",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_paymentInfos_users_UserId",
                table: "paymentInfos",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
