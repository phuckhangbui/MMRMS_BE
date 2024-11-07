using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class changePropertiesOfMachineSerialNumberComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ComponentId",
                table: "MachineSerialNumberComponent",
                newName: "MachineComponentId");

            migrationBuilder.RenameIndex(
                name: "IX_MachineSerialNumberComponent_ComponentId",
                table: "MachineSerialNumberComponent",
                newName: "IX_MachineSerialNumberComponent_MachineComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MachineComponentId",
                table: "MachineSerialNumberComponent",
                newName: "ComponentId");

            migrationBuilder.RenameIndex(
                name: "IX_MachineSerialNumberComponent_MachineComponentId",
                table: "MachineSerialNumberComponent",
                newName: "IX_MachineSerialNumberComponent_ComponentId");
        }
    }
}
