using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateComponentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaintainTicketId",
                table: "Invoices",
                newName: "ComponentReplacementTicketId");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Component",
                newName: "QuantityOnHold");

            migrationBuilder.AddColumn<int>(
                name: "AvailableQuantity",
                table: "Component",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableQuantity",
                table: "Component");

            migrationBuilder.RenameColumn(
                name: "ComponentReplacementTicketId",
                table: "Invoices",
                newName: "MaintainTicketId");

            migrationBuilder.RenameColumn(
                name: "QuantityOnHold",
                table: "Component",
                newName: "Quantity");
        }
    }
}
