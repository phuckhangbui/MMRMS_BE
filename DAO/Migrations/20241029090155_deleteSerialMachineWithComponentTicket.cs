using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class deleteSerialMachineWithComponentTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentReplacementTicket_MachineSerialNumber_MachineSerialNumberSerialNumber",
                table: "ComponentReplacementTicket");

            migrationBuilder.DropIndex(
                name: "IX_ComponentReplacementTicket_MachineSerialNumberSerialNumber",
                table: "ComponentReplacementTicket");

            migrationBuilder.DropColumn(
                name: "MachineSerialNumberSerialNumber",
                table: "ComponentReplacementTicket");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MachineSerialNumberSerialNumber",
                table: "ComponentReplacementTicket",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReplacementTicket_MachineSerialNumberSerialNumber",
                table: "ComponentReplacementTicket",
                column: "MachineSerialNumberSerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentReplacementTicket_MachineSerialNumber_MachineSerialNumberSerialNumber",
                table: "ComponentReplacementTicket",
                column: "MachineSerialNumberSerialNumber",
                principalTable: "MachineSerialNumber",
                principalColumn: "SerialNumber");
        }
    }
}
