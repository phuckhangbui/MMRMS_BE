using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateFieldContractAndRentingRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "DiscountShip",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "FinalAmount",
                table: "Contract");

            migrationBuilder.RenameColumn(
                name: "ShippingPrice",
                table: "Contract",
                newName: "TotalRentPrice");

            migrationBuilder.AddColumn<double>(
                name: "TotalServicePrice",
                table: "RentingRequest",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfMonth",
                table: "Contract",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalServicePrice",
                table: "RentingRequest");

            migrationBuilder.DropColumn(
                name: "NumberOfMonth",
                table: "Contract");

            migrationBuilder.RenameColumn(
                name: "TotalRentPrice",
                table: "Contract",
                newName: "ShippingPrice");

            migrationBuilder.AddColumn<double>(
                name: "DiscountPrice",
                table: "Contract",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiscountShip",
                table: "Contract",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FinalAmount",
                table: "Contract",
                type: "float",
                nullable: true);
        }
    }
}
