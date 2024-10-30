using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class modifyRelationshipBetweenCheckRequestAndTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MachineTask_MachineCheckRequestId",
                table: "MachineTask");

            migrationBuilder.AddColumn<int>(
                name: "MachineTaskId",
                table: "MachineCheckRequest",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_MachineCheckRequestId",
                table: "MachineTask",
                column: "MachineCheckRequestId",
                unique: true,
                filter: "[MachineCheckRequestId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MachineTask_MachineCheckRequestId",
                table: "MachineTask");

            migrationBuilder.DropColumn(
                name: "MachineTaskId",
                table: "MachineCheckRequest");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_MachineCheckRequestId",
                table: "MachineTask",
                column: "MachineCheckRequestId");
        }
    }
}
