using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class deleteMachineCheckRequestCriteriaConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MachineCheckRequestCriteria_MachineCheckCriteria",
                table: "MachineCheckRequestCriteria");

            migrationBuilder.DropIndex(
                name: "IX_MachineCheckRequestCriteria_MachineCheckCriteriaId",
                table: "MachineCheckRequestCriteria");

            migrationBuilder.RenameColumn(
                name: "MachineCheckCriteriaId",
                table: "MachineCheckRequestCriteria",
                newName: "CriteriaName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CriteriaName",
                table: "MachineCheckRequestCriteria",
                newName: "MachineCheckCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineCheckRequestCriteria_MachineCheckCriteriaId",
                table: "MachineCheckRequestCriteria",
                column: "MachineCheckCriteriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MachineCheckRequestCriteria_MachineCheckCriteria",
                table: "MachineCheckRequestCriteria",
                column: "MachineCheckCriteriaId",
                principalTable: "MachineCheckCriteria",
                principalColumn: "MachineCheckCriteriaId");
        }
    }
}
