using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addSerialMachineLogToSerialMachineComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MachineComponentStatusId",
                table: "MachineSerialNumberLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MachineSerialNumberComponentId",
                table: "MachineSerialNumberLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "MachineComponentStatus",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MachineSerialNumberLog_MachineComponentStatusId",
                table: "MachineSerialNumberLog",
                column: "MachineComponentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_MachineSerialNumberLog_MachineComponentStatus_MachineComponentStatusId",
                table: "MachineSerialNumberLog",
                column: "MachineComponentStatusId",
                principalTable: "MachineComponentStatus",
                principalColumn: "MachineComponentStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MachineSerialNumberLog_MachineComponentStatus_MachineComponentStatusId",
                table: "MachineSerialNumberLog");

            migrationBuilder.DropIndex(
                name: "IX_MachineSerialNumberLog_MachineComponentStatusId",
                table: "MachineSerialNumberLog");

            migrationBuilder.DropColumn(
                name: "MachineComponentStatusId",
                table: "MachineSerialNumberLog");

            migrationBuilder.DropColumn(
                name: "MachineSerialNumberComponentId",
                table: "MachineSerialNumberLog");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "MachineComponentStatus");
        }
    }
}
