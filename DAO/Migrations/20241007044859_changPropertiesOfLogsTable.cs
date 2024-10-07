using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class changPropertiesOfLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "TaskLog",
                newName: "AccountTriggerId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLog_AccountId",
                table: "TaskLog",
                newName: "IX_TaskLog_AccountTriggerId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "DeliveryLog",
                newName: "AccountTriggerId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryLog_AccountId",
                table: "DeliveryLog",
                newName: "IX_DeliveryLog_AccountTriggerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountTriggerId",
                table: "TaskLog",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLog_AccountTriggerId",
                table: "TaskLog",
                newName: "IX_TaskLog_AccountId");

            migrationBuilder.RenameColumn(
                name: "AccountTriggerId",
                table: "DeliveryLog",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryLog_AccountTriggerId",
                table: "DeliveryLog",
                newName: "IX_DeliveryLog_AccountId");
        }
    }
}
