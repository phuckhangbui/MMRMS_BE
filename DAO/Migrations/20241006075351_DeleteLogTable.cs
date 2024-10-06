using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class DeleteLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogDetail_LogID",
                table: "LogDetail");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.RenameColumn(
                name: "LogId",
                table: "LogDetail",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_LogDetail_LogId",
                table: "LogDetail",
                newName: "IX_LogDetail_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogDetail_AccountID",
                table: "LogDetail",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogDetail_AccountID",
                table: "LogDetail");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "LogDetail",
                newName: "LogId");

            migrationBuilder.RenameIndex(
                name: "IX_LogDetail_AccountId",
                table: "LogDetail",
                newName: "IX_LogDetail_LogId");

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountLogId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Log_Account",
                        column: x => x.AccountLogId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Log_AccountLogId",
                table: "Log",
                column: "AccountLogId",
                unique: true,
                filter: "[AccountLogId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_LogDetail_LogID",
                table: "LogDetail",
                column: "LogId",
                principalTable: "Log",
                principalColumn: "LogId");
        }
    }
}
