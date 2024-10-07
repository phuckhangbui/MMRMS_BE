using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addTaskAndRequestResponseRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestDateResponse");

            migrationBuilder.AddColumn<int>(
                name: "RequestResponseId",
                table: "EmployeeTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RequestResponse",
                columns: table => new
                {
                    ResponseDateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    EmployeeTaskId = table.Column<int>(type: "int", nullable: true),
                    DateResponse = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestResponse", x => x.ResponseDateId);
                    table.ForeignKey(
                        name: "FK_RequestResponse_MaintenanceRequest",
                        column: x => x.RequestId,
                        principalTable: "MaintenanceRequest",
                        principalColumn: "RequestId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTask_RequestResponseId",
                table: "EmployeeTask",
                column: "RequestResponseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestResponse_RequestId",
                table: "RequestResponse",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Response",
                table: "EmployeeTask",
                column: "RequestResponseId",
                principalTable: "RequestResponse",
                principalColumn: "ResponseDateId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Response",
                table: "EmployeeTask");

            migrationBuilder.DropTable(
                name: "RequestResponse");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeTask_RequestResponseId",
                table: "EmployeeTask");

            migrationBuilder.DropColumn(
                name: "RequestResponseId",
                table: "EmployeeTask");

            migrationBuilder.CreateTable(
                name: "RequestDateResponse",
                columns: table => new
                {
                    ResponseDateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateResponse = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDateResponse", x => x.ResponseDateId);
                    table.ForeignKey(
                        name: "FK_RequestDateResponse_MaintenanceRequest",
                        column: x => x.RequestId,
                        principalTable: "MaintenanceRequest",
                        principalColumn: "RequestId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestDateResponse_RequestId",
                table: "RequestDateResponse",
                column: "RequestId");
        }
    }
}
