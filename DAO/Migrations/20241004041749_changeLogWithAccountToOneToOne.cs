using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class changeLogWithAccountToOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_AccountLogID",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Log_AccountLogId",
                table: "Log");

            migrationBuilder.RenameColumn(
                name: "AccountPromotionId",
                table: "Account",
                newName: "LogId");

            migrationBuilder.AddColumn<string>(
                name: "NotificationTitle",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Log_AccountLogId",
                table: "Log",
                column: "AccountLogId",
                unique: true,
                filter: "[AccountLogId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Log_Account",
                table: "Log",
                column: "AccountLogId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_Account",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Log_AccountLogId",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "NotificationTitle",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "LogId",
                table: "Account",
                newName: "AccountPromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_AccountLogId",
                table: "Log",
                column: "AccountLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Log_AccountLogID",
                table: "Log",
                column: "AccountLogId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }
    }
}
