using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class deleteRequestResponseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Response",
                table: "MachineTask");

            migrationBuilder.DropTable(
                name: "RequestResponse");

            migrationBuilder.DropIndex(
                name: "IX_MachineTask_RequestResponseId",
                table: "MachineTask");

            migrationBuilder.DropColumn(
                name: "RequestResponseId",
                table: "MachineTask");

            migrationBuilder.AddColumn<string>(
                name: "MachineCheckRequestId",
                table: "MachineTask",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_MachineCheckRequestId",
                table: "MachineTask",
                column: "MachineCheckRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_MachineCheckRequest",
                table: "MachineTask",
                column: "MachineCheckRequestId",
                principalTable: "MachineCheckRequest",
                principalColumn: "MachineCheckRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_MachineCheckRequest",
                table: "MachineTask");

            migrationBuilder.DropIndex(
                name: "IX_MachineTask_MachineCheckRequestId",
                table: "MachineTask");

            migrationBuilder.DropColumn(
                name: "MachineCheckRequestId",
                table: "MachineTask");

            migrationBuilder.AddColumn<int>(
                name: "RequestResponseId",
                table: "MachineTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RequestResponse",
                columns: table => new
                {
                    RequestResponseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineCheckRequestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateResponse = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineTaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestResponse", x => x.RequestResponseId);
                    table.ForeignKey(
                        name: "FK_RequestResponse_MachineCheckRequest",
                        column: x => x.MachineCheckRequestId,
                        principalTable: "MachineCheckRequest",
                        principalColumn: "MachineCheckRequestId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachineTask_RequestResponseId",
                table: "MachineTask",
                column: "RequestResponseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestResponse_MachineCheckRequestId",
                table: "RequestResponse",
                column: "MachineCheckRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Response",
                table: "MachineTask",
                column: "RequestResponseId",
                principalTable: "RequestResponse",
                principalColumn: "RequestResponseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
