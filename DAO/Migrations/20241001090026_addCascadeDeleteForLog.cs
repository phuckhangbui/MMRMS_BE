using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addCascadeDeleteForLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryLog_AccountID",
                table: "DeliveryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLog_TaskID",
                table: "TaskLog");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryLog_AccountID",
                table: "DeliveryLog",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLog_TaskID",
                table: "TaskLog",
                column: "EmployeeTaskId",
                principalTable: "EmployeeTask",
                principalColumn: "EmployeeTaskId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryLog_AccountID",
                table: "DeliveryLog");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLog_TaskID",
                table: "TaskLog");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryLog_AccountID",
                table: "DeliveryLog",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLog_TaskID",
                table: "TaskLog",
                column: "EmployeeTaskId",
                principalTable: "EmployeeTask",
                principalColumn: "EmployeeTaskId");
        }
    }
}
