using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class deleteComponentReplacementTicketWithMachineTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MachineTask_ComponentReplacementTicket_ComponentReplacementTicketId",
                table: "MachineTask");

            migrationBuilder.DropIndex(
                name: "IX_MachineTask_ComponentReplacementTicketId",
                table: "MachineTask");

            migrationBuilder.DropColumn(
                name: "ComponentReplacementTicketId",
                table: "MachineTask");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComponentReplacementTicketId",
                table: "MachineTask",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_ComponentReplacementTicketId",
                table: "MachineTask",
                column: "ComponentReplacementTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_MachineTask_ComponentReplacementTicket_ComponentReplacementTicketId",
                table: "MachineTask",
                column: "ComponentReplacementTicketId",
                principalTable: "ComponentReplacementTicket",
                principalColumn: "ComponentReplacementTicketId");
        }
    }
}
