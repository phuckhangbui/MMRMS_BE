using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class editMachineTaskProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentReplacementTicketId_Task",
                table: "MachineTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_PreviousTask",
                table: "MachineTask");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropIndex(
                name: "IX_MachineTask_PreviousTaskId",
                table: "MachineTask");

            migrationBuilder.DropColumn(
                name: "PreviousTaskId",
                table: "MachineTask");

            migrationBuilder.AddForeignKey(
                name: "FK_MachineTask_ComponentReplacementTicket_ComponentReplacementTicketId",
                table: "MachineTask",
                column: "ComponentReplacementTicketId",
                principalTable: "ComponentReplacementTicket",
                principalColumn: "ComponentReplacementTicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MachineTask_ComponentReplacementTicket_ComponentReplacementTicketId",
                table: "MachineTask");

            migrationBuilder.AddColumn<int>(
                name: "PreviousTaskId",
                table: "MachineTask",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineTaskId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReportContent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Report_TaskID",
                        column: x => x.MachineTaskId,
                        principalTable: "MachineTask",
                        principalColumn: "MachineTaskId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_PreviousTaskId",
                table: "MachineTask",
                column: "PreviousTaskId",
                unique: true,
                filter: "[PreviousTaskId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Report_MachineTaskId",
                table: "Report",
                column: "MachineTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentReplacementTicketId_Task",
                table: "MachineTask",
                column: "ComponentReplacementTicketId",
                principalTable: "ComponentReplacementTicket",
                principalColumn: "ComponentReplacementTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_PreviousTask",
                table: "MachineTask",
                column: "PreviousTaskId",
                principalTable: "MachineTask",
                principalColumn: "MachineTaskId");
        }
    }
}
