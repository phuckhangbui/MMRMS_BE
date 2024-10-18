using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addRelationshipBetweenTaskAndMaintenanceReqeustWhenCreateMaintenanceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeTaskCreateId",
                table: "MaintenanceTicket",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTicket_EmployeeTaskCreateId",
                table: "MaintenanceTicket",
                column: "EmployeeTaskCreateId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTicket_EmployeeTaskCreated",
                table: "MaintenanceTicket",
                column: "EmployeeTaskCreateId",
                principalTable: "EmployeeTask",
                principalColumn: "EmployeeTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTicket_EmployeeTaskCreated",
                table: "MaintenanceTicket");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceTicket_EmployeeTaskCreateId",
                table: "MaintenanceTicket");

            migrationBuilder.DropColumn(
                name: "EmployeeTaskCreateId",
                table: "MaintenanceTicket");
        }
    }
}
