using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class modifyRelationshipBetweenMantenanceAndTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTicket_TaskID",
                table: "MaintenanceTicket");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceTicket_EmployeeTaskId",
                table: "MaintenanceTicket");

            migrationBuilder.DropColumn(
                name: "EmployeeTaskId",
                table: "MaintenanceTicket");

            migrationBuilder.AddColumn<int>(
                name: "MaintenanceTicketId",
                table: "EmployeeTask",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTask_MaintenanceTicketId",
                table: "EmployeeTask",
                column: "MaintenanceTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTicketId_Task",
                table: "EmployeeTask",
                column: "MaintenanceTicketId",
                principalTable: "MaintenanceTicket",
                principalColumn: "MaintenanceTicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTicketId_Task",
                table: "EmployeeTask");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeTask_MaintenanceTicketId",
                table: "EmployeeTask");

            migrationBuilder.DropColumn(
                name: "MaintenanceTicketId",
                table: "EmployeeTask");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeTaskId",
                table: "MaintenanceTicket",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTicket_EmployeeTaskId",
                table: "MaintenanceTicket",
                column: "EmployeeTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTicket_TaskID",
                table: "MaintenanceTicket",
                column: "EmployeeTaskId",
                principalTable: "EmployeeTask",
                principalColumn: "EmployeeTaskId");
        }
    }
}
