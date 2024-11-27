using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addPropertiesContractTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RefundShippingPrice",
                table: "Contract",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ShippingDistance",
                table: "Contract",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ShippingPrice",
                table: "Contract",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefundShippingPrice",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ShippingDistance",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ShippingPrice",
                table: "Contract");
        }
    }
}
