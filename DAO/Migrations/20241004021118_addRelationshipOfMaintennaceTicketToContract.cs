using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addRelationshipOfMaintennaceTicketToContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractId",
                table: "MaintenanceTicket",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTicket_ContractId",
                table: "MaintenanceTicket",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTicket_ContractID",
                table: "MaintenanceTicket",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTicket_ContractID",
                table: "MaintenanceTicket");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceTicket_ContractId",
                table: "MaintenanceTicket");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "MaintenanceTicket");
        }
    }
}
