using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class DeleteRelationshipBetweenMaintenceRequestAndSerialNumberProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequest_SerialNumberProduct",
                table: "MaintenanceRequest");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceRequest_SerialNumber",
                table: "MaintenanceRequest");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "MaintenanceRequest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "MaintenanceRequest",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_SerialNumber",
                table: "MaintenanceRequest",
                column: "SerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequest_SerialNumberProduct",
                table: "MaintenanceRequest",
                column: "SerialNumber",
                principalTable: "SerialNumberProduct",
                principalColumn: "SerialNumber");
        }
    }
}
