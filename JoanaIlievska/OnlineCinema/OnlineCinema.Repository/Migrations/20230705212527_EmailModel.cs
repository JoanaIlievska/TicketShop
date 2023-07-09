using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCinema.Repository.Migrations
{
    public partial class EmailModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketsInShoppingCarts",
                table: "TicketsInShoppingCarts");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TicketInOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketsInShoppingCarts",
                table: "TicketsInShoppingCarts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MailTo = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketsInShoppingCarts_ProductId",
                table: "TicketsInShoppingCarts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketsInShoppingCarts",
                table: "TicketsInShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_TicketsInShoppingCarts_ProductId",
                table: "TicketsInShoppingCarts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TicketInOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketsInShoppingCarts",
                table: "TicketsInShoppingCarts",
                columns: new[] { "ProductId", "ShoppingCartId" });
        }
    }
}
