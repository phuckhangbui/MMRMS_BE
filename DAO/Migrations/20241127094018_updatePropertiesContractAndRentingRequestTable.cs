using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updatePropertiesContractAndRentingRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingDistance",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ShippingPrice",
                table: "Contract");

            migrationBuilder.AddColumn<double>(
                name: "ShippingDistance",
                table: "RentingRequest",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ShippingPricePerKm",
                table: "RentingRequest",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingDistance",
                table: "RentingRequest");

            migrationBuilder.DropColumn(
                name: "ShippingPricePerKm",
                table: "RentingRequest");

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
    }
}
