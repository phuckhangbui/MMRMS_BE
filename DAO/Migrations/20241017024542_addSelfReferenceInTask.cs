using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addSelfReferenceInTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreviousTaskId",
                table: "EmployeeTask",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTask_PreviousTaskId",
                table: "EmployeeTask",
                column: "PreviousTaskId",
                unique: true,
                filter: "[PreviousTaskId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_PreviousTask",
                table: "EmployeeTask",
                column: "PreviousTaskId",
                principalTable: "EmployeeTask",
                principalColumn: "EmployeeTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_PreviousTask",
                table: "EmployeeTask");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeTask_PreviousTaskId",
                table: "EmployeeTask");

            migrationBuilder.DropColumn(
                name: "PreviousTaskId",
                table: "EmployeeTask");
        }
    }
}
