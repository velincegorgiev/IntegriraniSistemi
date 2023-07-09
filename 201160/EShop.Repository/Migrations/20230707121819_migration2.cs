using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.Repository.Migrations
{
    public partial class migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInOrders",
                table: "TicketInOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TickedInCards",
                table: "TickedInCards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInOrders",
                table: "TicketInOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TickedInCards",
                table: "TickedInCards",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInOrders_TicketId",
                table: "TicketInOrders",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TickedInCards_TicketId",
                table: "TickedInCards",
                column: "TicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketInOrders",
                table: "TicketInOrders");

            migrationBuilder.DropIndex(
                name: "IX_TicketInOrders_TicketId",
                table: "TicketInOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TickedInCards",
                table: "TickedInCards");

            migrationBuilder.DropIndex(
                name: "IX_TickedInCards_TicketId",
                table: "TickedInCards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketInOrders",
                table: "TicketInOrders",
                columns: new[] { "TicketId", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TickedInCards",
                table: "TickedInCards",
                columns: new[] { "TicketId", "CardId" });
        }
    }
}
