using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateTableNameOfMachineComponentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentReplacementTicket_MachineSerialNumber",
                table: "ComponentReplacementTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_MachineSerialNumberLog_MachineComponentStatus_MachineComponentStatusId",
                table: "MachineSerialNumberLog");

            migrationBuilder.DropTable(
                name: "MachineComponentStatus");

            migrationBuilder.DropIndex(
                name: "IX_MachineSerialNumberLog_MachineComponentStatusId",
                table: "MachineSerialNumberLog");

            migrationBuilder.DropColumn(
                name: "MachineComponentStatusId",
                table: "MachineSerialNumberLog");

            migrationBuilder.DropColumn(
                name: "IsRequiredMoney",
                table: "MachineComponent");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MachineComponent");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "ComponentReplacementTicket",
                newName: "MachineSerialNumberSerialNumber");

            migrationBuilder.RenameIndex(
                name: "IX_ComponentReplacementTicket_SerialNumber",
                table: "ComponentReplacementTicket",
                newName: "IX_ComponentReplacementTicket_MachineSerialNumberSerialNumber");

            migrationBuilder.AlterColumn<int>(
                name: "MachineSerialNumberComponentId",
                table: "MachineSerialNumberLog",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MachineSerialNumberComponentId",
                table: "ComponentReplacementTicket",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MachineSerialNumberComponent",
                columns: table => new
                {
                    MachineSerialNumberComponentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ComponentId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineSerialNumberComponent", x => x.MachineSerialNumberComponentId);
                    table.ForeignKey(
                        name: "FK_MachineSerialNumberComponent_ComponentID",
                        column: x => x.ComponentId,
                        principalTable: "MachineComponent",
                        principalColumn: "MachineComponentId");
                    table.ForeignKey(
                        name: "FK_MachineSerialNumberComponent_MachineSerialNumber",
                        column: x => x.SerialNumber,
                        principalTable: "MachineSerialNumber",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachineSerialNumberLog_MachineSerialNumberComponentId",
                table: "MachineSerialNumberLog",
                column: "MachineSerialNumberComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReplacementTicket_MachineSerialNumberComponentId",
                table: "ComponentReplacementTicket",
                column: "MachineSerialNumberComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineSerialNumberComponent_ComponentId",
                table: "MachineSerialNumberComponent",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineSerialNumberComponent_SerialNumber",
                table: "MachineSerialNumberComponent",
                column: "SerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentReplacementTicket_MachineSerialNumberComponent",
                table: "ComponentReplacementTicket",
                column: "MachineSerialNumberComponentId",
                principalTable: "MachineSerialNumberComponent",
                principalColumn: "MachineSerialNumberComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentReplacementTicket_MachineSerialNumber_MachineSerialNumberSerialNumber",
                table: "ComponentReplacementTicket",
                column: "MachineSerialNumberSerialNumber",
                principalTable: "MachineSerialNumber",
                principalColumn: "SerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_MachineSerialNumberComponent_Log",
                table: "MachineSerialNumberLog",
                column: "MachineSerialNumberComponentId",
                principalTable: "MachineSerialNumberComponent",
                principalColumn: "MachineSerialNumberComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentReplacementTicket_MachineSerialNumberComponent",
                table: "ComponentReplacementTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentReplacementTicket_MachineSerialNumber_MachineSerialNumberSerialNumber",
                table: "ComponentReplacementTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_MachineSerialNumberComponent_Log",
                table: "MachineSerialNumberLog");

            migrationBuilder.DropTable(
                name: "MachineSerialNumberComponent");

            migrationBuilder.DropIndex(
                name: "IX_MachineSerialNumberLog_MachineSerialNumberComponentId",
                table: "MachineSerialNumberLog");

            migrationBuilder.DropIndex(
                name: "IX_ComponentReplacementTicket_MachineSerialNumberComponentId",
                table: "ComponentReplacementTicket");

            migrationBuilder.DropColumn(
                name: "MachineSerialNumberComponentId",
                table: "ComponentReplacementTicket");

            migrationBuilder.RenameColumn(
                name: "MachineSerialNumberSerialNumber",
                table: "ComponentReplacementTicket",
                newName: "SerialNumber");

            migrationBuilder.RenameIndex(
                name: "IX_ComponentReplacementTicket_MachineSerialNumberSerialNumber",
                table: "ComponentReplacementTicket",
                newName: "IX_ComponentReplacementTicket_SerialNumber");

            migrationBuilder.AlterColumn<string>(
                name: "MachineSerialNumberComponentId",
                table: "MachineSerialNumberLog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MachineComponentStatusId",
                table: "MachineSerialNumberLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredMoney",
                table: "MachineComponent",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MachineComponent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MachineComponentStatus",
                columns: table => new
                {
                    MachineComponentStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentId = table.Column<int>(type: "int", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineComponentStatus", x => x.MachineComponentStatusId);
                    table.ForeignKey(
                        name: "FK_MachineComponentStatus_ComponentID",
                        column: x => x.ComponentId,
                        principalTable: "MachineComponent",
                        principalColumn: "MachineComponentId");
                    table.ForeignKey(
                        name: "FK_MachineComponentStatus_MachineSerialNumber",
                        column: x => x.SerialNumber,
                        principalTable: "MachineSerialNumber",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachineSerialNumberLog_MachineComponentStatusId",
                table: "MachineSerialNumberLog",
                column: "MachineComponentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineComponentStatus_ComponentId",
                table: "MachineComponentStatus",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineComponentStatus_SerialNumber",
                table: "MachineComponentStatus",
                column: "SerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentReplacementTicket_MachineSerialNumber",
                table: "ComponentReplacementTicket",
                column: "SerialNumber",
                principalTable: "MachineSerialNumber",
                principalColumn: "SerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_MachineSerialNumberLog_MachineComponentStatus_MachineComponentStatusId",
                table: "MachineSerialNumberLog",
                column: "MachineComponentStatusId",
                principalTable: "MachineComponentStatus",
                principalColumn: "MachineComponentStatusId");
        }
    }
}
